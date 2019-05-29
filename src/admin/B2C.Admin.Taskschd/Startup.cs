using B2C.Admin.Taskschd.Tiggers;
using B2C.Message.Contract.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Never;
using Never.Commands;
using Never.Configuration;
using Never.Configuration.ConfigCenter;
using Never.Configuration.ConfigCenter.Remoting;
using Never.Deployment;
using Never.IoC;
using Never.Logging;
using Never.NLog;
using Never.QuartzNET;
using Never.Serialization;
using Never.Serialization.Json;
using Never.SqliteRecovery;
using Never.Web.WebApi;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;


namespace B2C.Admin.Taskschd
{
    public class Startup : Never.Web.WebApi.WebStartup
    {
        public Startup(IConfiguration configuration,
            IHostingEnvironment environment) : base(() => new Never.Web.WebApplicationStartup(new Never.Web.IoC.Providers.WebDomainAssemblyProvider(GetAssemblyName)))
        {
            this.Configuration = configuration;
            this.Environment = environment;
            this.OnStarting += this.Startup_OnStarting;
        }

        private void Startup_OnStarting(object sender, Never.StartupEventArgs e)
        {
            var commandfile = new FileInfo(AppContext.BaseDirectory + "\\App_Data\\command_demo.db");
            var eventfile = new FileInfo(AppContext.BaseDirectory + "\\App_Data\\event_demo.db");
            var logfile = new FileInfo(AppContext.BaseDirectory + "\\App_Config\\nlog.config");
            var configReader = new AppConfigReader(this.Configuration);

            /*json序列化配置*/
            DefaultSetting.DefaultDeserializeSetting = new JsonDeserializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };
            DefaultSetting.DefaultSerializeSetting = new JsonSerializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };

            e.Startup.RegisterAssemblyFilter("B2C".CreateAssemblyFilter()).RegisterAssemblyFilter("Never".CreateAssemblyFilter());
            e.Startup.UseEasyIoC(
                (x, y, z) =>
                {
                    var mockTest = new Never.Aop.DynamicProxy.Mock<ITest>();
                    mockTest.Setup(m => m.Write(DateTime.Now)).Void(m => 
                    {
                        Console.WriteLine(DateTime.Now);
                    });

                    x.RegisterInstance(mockTest.CreateIllusive(), typeof(ITest));
                },
                (x, y, z) =>
                {
                    if (configReader["application"].IsNullOrWhiteSpace())
                    {
                        throw new Exception("读取配置文件为空，是否配置中心未更新配置文件");
                    }
                });

            e.Startup.UseAutoInjectingAttributeUsingIoC(new IAutoInjectingEnvironmentProvider[]
            {
                //由于没有注入Repository与Query，所以不用加自动注册属性了
                //SingletonAutoInjectingEnvironmentProvider.UsingRuleContainerAutoInjectingEnvironmentProvider("message"),
            })
            .UseConfigClient(new IPEndPoint(IPAddress.Parse(configReader["config_host"]), configReader.IntInAppConfig("config_port")), out var configFileClient);
            configFileClient.Startup(TimeSpan.FromMinutes(10), new[] { new ConfigFileClientRequest { FileName = "admin_task" } }, (c, t) =>
            {
                var content = t;
                if (c != null && c.FileName == "admin_task")
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(this.Environment.ContentRootPath, "appsettings.app.json"), content);
                }
            }).Push("admin_task").GetAwaiter().GetResult();

            e.Startup.UseApiUriRouteDispatch(40, (x) => new IApiRouteProvider[]
            {
                new B2C.Message.Contract.Services.ApiRouteProvider(configReader),
            }, () => e.Startup.ServiceLocator.ResolveOptional<ILoggerBuilder>())
            .UseHttpProxyGenerateMessageApi();

            e.Startup
                .UseCounterCache()
                .UseConcurrentCache()
                .UseDataContractJson()
                .UseEasyJson(string.Empty)
                .UseNLog(logfile)
                .UseQuartzNET<LoggerHealthReport>()
                .UseAppConfig(configReader)
                .UseForceCheckAggregateRootImplIHandle()
                .UseForceCheckCommandAppDomainAttribute()
                .UseForceCheckCommandEvenWithNoParamaterCtor()
                .UseForceCheckCommandHandlerCtor()
                .UseForceCheckEventAppDomainAttribute()
                .UseForceCheckEventHandlerCtor()
                .UseForceCheckMessageSubscriberCtor()
                .UseInjectingCommandHandlerEventHandler(Never.IoC.ComponentLifeStyle.Singleton)
                .UseSqliteEventProviderCommandBus<DefaultCommandContext>(new SqliteFailRecoveryStorager(commandfile, eventfile))
                .UseApiModelStateValidation()
                .UseApiActionCustomRoute(e.Collector as IServiceCollection)
                .UseApiDependency(e.Collector as IServiceCollection);

            //配置中心更新配置文件后，系统不一定马上能重新加载
            e.Startup.Startup(TimeSpan.FromSeconds(1), (x) =>
            {
                //test
                using (var sc = x.ServiceLocator.BeginLifetimeScope())
                {
                    sc.Resolve<ICommandBus>();
                    sc.Resolve<ILoggerBuilder>();
                    sc.Resolve<IJsonSerializer>();
                    sc.Resolve<ITest>();
                    sc.Resolve<Worker>();
                    var logger = sc.Resolve<ILoggerBuilder>().Build(typeof(Startup));
                    logger.Info("startup at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            });
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

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
                x.InputFormatters.Add(new JsonFormatter());
                x.OutputFormatters.Add(new JsonFormatter());
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
            if (file.IndexOf("B2C.Admin.Taskschd.Views.dll", StringComparison.OrdinalIgnoreCase) > 0)
                return false;

            return true;
        }
    }
}
