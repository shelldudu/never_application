using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Never;
using Never.Commands;
using Never.Configuration;
using Never.Configuration.ConfigCenter;
using Never.Configuration.ConfigCenter.Remoting;
using Never.IoC;
using Never.Logging;
using Never.NLog;
using Never.Serialization;
using Never.Serialization.Json;
using Never.SqliteRecovery;
using Never.Web.WebApi;
using B2C.Message.Contract.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace B2C.Message.Api
{
    /// <summary>
    /// 继承该类是比较方便加入构架代码，实际上也可以不用继承，只要在系统启动之前调用ApplicationStartup的Startup方法就可以了
    /// </summary>
    public class Startup : Never.Web.WebApi.WebStartup
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <remarks>使用回调，将在ConfigureServices方法过路中再构造ApplicationStartup对象，由于我们是使用webapi开发，所以我们选择了WebApplicationStartup</remarks>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment) : base(() => new Never.Web.WebApplicationStartup(new Never.Web.IoC.Providers.WebDomainAssemblyProvider(GetAssemblyName)))
        {
            this.Configuration = configuration;
            this.Environment = environment;
            this.OnStarting += this.Startup_OnStarting;
        }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        /// <summary>
        /// 该方法被ConfigureServices里面的base.ConfigureServicese调用，由于ConfigureServices方法会使用不同的组件方案，所以在其后面启支，是将这些组件方案所注册的ioc规则加入到自己的ioc规则里面去
        /// 同时替换了系统IServiceCollection自己生成的IServiceProvider对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Startup_OnStarting(object sender, Never.StartupEventArgs e)
        {
            //ddd的command里面使用了恢复（即一些命令出错后被保存后过段时间再执行），当前使用sqlite本地数据库方式
            var commandfile = new FileInfo(AppContext.BaseDirectory + "\\App_Data\\command_demo.db");
            //ddd的event跟上面的一样
            var eventfile = new FileInfo(AppContext.BaseDirectory + "\\App_Data\\event_demo.db");
            //使用nlog组件
            var logfile = new FileInfo(AppContext.BaseDirectory + "\\App_Config\\nlog.config");
            //配置文件的读取
            var configReader = new AppConfigReader(this.Configuration);

            /*json序列化配置*/
            DefaultSetting.DefaultDeserializeSetting = new JsonDeserializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };
            DefaultSetting.DefaultSerializeSetting = new JsonSerializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };

            //注册程序集过滤，因为整个启动过程会分析程序集里面的Type对象，很多dll我们不用分析，只焦点到我们现在注入的2个规则就行，"Never" + "B2C",正则只要匹配到该字符就加加载到待分析的dll集合中
            e.Startup.RegisterAssemblyFilter("B2C".CreateAssemblyFilter()).RegisterAssemblyFilter("Never".CreateAssemblyFilter());

            //ioc分2种启动方法，主要原因如下：（1）服务启动有先后顺序，不同的系统组件所注册的顺序不同的，但有些组件要求在所有环境下都只有第一或最后启动（2）由于使用环境自动注册这种设计下，一些组件要手动注册会带自己的规则就会被自动注册覆盖
            e.Startup.UseEasyIoC(
                (x, y, z) =>
                {
                    //先启动该服务注册组件，
                },
                (x, y, z) =>
                {
                    //再按自己的个性化注册组件，比如Controller在下面UseApiDependency后会自动注入，但是我想HomeController注入的时候使用memecahed，这种情况就要手动注入了
                    //x.RegisterType<Controllers.HomeController, Controllers.HomeController>().WithParameter<Never.Caching.ICaching>("memcached");

                    //注入query与repository实例，为什么不用自动注入？哈哈，因为在framework或netcore等各种不同的环境下大家读取配置文件是不同的，一旦写死在B2C.Message.SqlData.Query里面读取配置文件，则使用不同的host技术就出现极大问题，
                    //比如netcore没有connectionString这种配置（或者有人说可以手动引用System.Configuration，这不是嫌麻烦吗）
                    x.RegisterInstance(new B2C.Message.SqlData.Query.QueryDaoBuilder(Infrastructure.SqldbType.sqlserver, () => configReader["message_conn"]));
                    x.RegisterInstance(new B2C.Message.SqlData.Repository.RepositoryDaoBuilder(Infrastructure.SqldbType.sqlserver, () => configReader["message_conn"]));
                });

            //使用环境下自动注册组件，
            e.Startup.UseAutoInjectingAttributeUsingIoC(new IAutoInjectingEnvironmentProvider[]
            {
                //在message该环境下，所有单例注册组件只有匹配message的才注册，（1）有些组件是线程的，那么不会被描述和注入中，除非再加个线程provider；（2）即使是单例provider,但所运行不是message环境，所以也不会注入
                SingletonAutoInjectingEnvironmentProvider.UsingRuleContainerAutoInjectingEnvironmentProvider("message"),
            })
            //使用统一配置中心读取配置文件，实用性在后面有讲到
            .UseConfigClient(new IPEndPoint(IPAddress.Parse(configReader["config_host"]), configReader.IntInAppConfig("config_port")), out var configFileClient);
            configFileClient.Startup(TimeSpan.FromMinutes(10), new[] { new ConfigFileClientRequest { FileName = "message_api" } }, (c, t) =>
            {
                var content = t;
                if (c != null && c.FileName == "message_api")
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(this.Environment.ContentRootPath, "appsettings.app.json"), content);
                }
            }).Push("message_api").GetAwaiter().GetResult();


            e.Startup
                .UseCounterCache() //使用countcache
                .UseConcurrentCache() //使用安全countcache
                .UseDataContractJson() //使用datacontract技术的序列化，实现了IJsonSerialize接口
                .UseEasyJson(string.Empty) //使用easyjson技术的序列化，实现了IJsonSerialize接口
                .UseNLog(logfile) //使用nlog
                .UseAppConfig(configReader) //将IConfigReader注入
                .UseForceCheckAggregateRootImplIHandle() //这几个Force都是为了检查ddd开发一些要求，比如是否继承某个类，某些接口
                .UseForceCheckCommandAppDomainAttribute() //检查所有的command是否带了特定attribute
                .UseForceCheckCommandEvenWithNoParamaterCtor() //检查所有的commandhandler所要的构造参数是否被注入中
                .UseForceCheckCommandHandlerCtor() //检查所有的eventhandler所要的构造参数是否被注入中
                .UseForceCheckEventAppDomainAttribute()//检查所有的event是否带了特定attribute
                .UseForceCheckEventHandlerCtor() //检查所有的eventhandler所要的构造参数是否被注入中
                .UseForceCheckMessageSubscriberCtor() //使用消息的订单与发布
                .UseInjectingCommandHandlerEventHandler(Never.IoC.ComponentLifeStyle.Singleton) //注入所有的commandhandler，在commandbus执行其对象行为
                .UseSqliteEventProviderCommandBus<DefaultCommandContext>(new SqliteFailRecoveryStorager(commandfile, eventfile)) //使用cqrs组件，指定sqlite作为恢复组件，
                .UseApiModelStateValidation() //mvc,webapi的模型参数验证
                .UseApiActionCustomRoute(e.Collector as IServiceCollection) //自定义路由，相同于在controller可以使用httpget等route技术
                .UseApiDependency(e.Collector as IServiceCollection);//注入所有的controller

            //配置中心更新配置文件后，系统不一定马上能重新加载
            e.Startup.Startup(TimeSpan.FromSeconds(1), (x) =>
            {
                //我们在此启动看看所使用组件是否正常启动
                using (var sc = x.ServiceLocator.BeginLifetimeScope())
                {
                    sc.Resolve<ICommandBus>();
                    sc.Resolve<ILoggerBuilder>();
                    sc.Resolve<IJsonSerializer>();
                    var home = sc.Resolve<Controllers.MessageController>();

                    var logger = sc.Resolve<ILoggerBuilder>().Build(typeof(Startup));
                    logger.Info("startup at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc((x) =>
            {
                x.OutputFormatters.Add(new JsonFormatter());
                x.InputFormatters.Add(new JsonFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return base.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = (context) =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null && feature.Error != null)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var loggerbuilder = context.RequestServices.GetServiceOptional<ILoggerBuilder>();
                        var logger = loggerbuilder.Build(typeof(Startup));
                        var url = UriHelper.BuildRelative(context.Request.PathBase, context.Request.Path, context.Request.QueryString, default(FragmentString));
                        logger.Error(string.Format("请求接口报错：{0}", url), feature.Error);
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }

                    return Task.CompletedTask;
                }
            });
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Get}/{id?}");
            });
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected static bool GetAssemblyName(string file)
        {
            if (file.IndexOf("B2C.Message.Api.Views.dll", StringComparison.OrdinalIgnoreCase) > 0)
                return false;

            return true;
        }
    }
}