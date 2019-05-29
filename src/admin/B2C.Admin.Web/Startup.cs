using B2C.Admin.Web.Permissions;
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
using Never.Configuration;
using Never.Configuration.ConfigCenter;
using Never.Configuration.ConfigCenter.Remoting;
using Never.Deployment;
using Never.IoC;
using Never.Logging;
using Never.NLog;
using Never.Serialization.Json;
using Never.SqliteRecovery;
using Never.Web.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace B2C.Admin.Web
{
    public class Startup : Never.Web.Mvc.WebStartup
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
            //use sqlite
            SQLitePCL.Batteries.Init();
            var sqliteFile = new FileInfo(AppContext.BaseDirectory + "\\App_Data\\b2c_admin_sqlite.db");

            var commandfile = new FileInfo(AppContext.BaseDirectory + "\\App_Data\\command_demo.db");
            var eventfile = new FileInfo(AppContext.BaseDirectory + "\\App_Data\\event_demo.db");
            var logfile = new FileInfo(AppContext.BaseDirectory + "\\App_Config\\nlog.config");
            var configReader = new AppConfigReader(this.Configuration);

            DefaultSetting.DefaultDeserializeSetting = new JsonDeserializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };
            DefaultSetting.DefaultSerializeSetting = new JsonSerializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle, WriteNumberOnBoolenType = false };

            e.Startup.RegisterAssemblyFilter("B2C".CreateAssemblyFilter()).RegisterAssemblyFilter("Never".CreateAssemblyFilter());
            e.Startup.UseEasyIoC(
                (x, y, z) =>
                {
                    x.RegisterType<FormsAuthenticationService, IAuthenticationService>(string.Empty, ComponentLifeStyle.Singleton);
                },
                (x, y, z) =>
                {
                    //prm
                    //x.RegisterInstance(new B2C.Admin.Web.Permissions.QueryDaoBuilder(Infrastructure.SqldbType.sqlserver, () => configReader["prm_conn"]));
                    //x.RegisterInstance(new B2C.Admin.Web.Permissions.RepositoryDaoBuilder(Infrastructure.SqldbType.sqlserver, () => configReader["prm_conn"]));

                    //x.RegisterInstance(new B2C.Admin.Web.Permissions.QueryDaoBuilder(Infrastructure.SqldbType.mysql, () => configReader["prm_conn"]));
                    //x.RegisterInstance(new B2C.Admin.Web.Permissions.RepositoryDaoBuilder(Infrastructure.SqldbType.mysql, () => configReader["prm_conn"]));

                    x.RegisterInstance(new B2C.Admin.Web.Permissions.QueryDaoBuilder(Infrastructure.SqldbType.sqlite, () => string.Concat("data source=", sqliteFile.FullName)));
                    x.RegisterInstance(new B2C.Admin.Web.Permissions.RepositoryDaoBuilder(Infrastructure.SqldbType.sqlite, () => string.Concat("data source=", sqliteFile.FullName)));

                    //message
                    x.RegisterInstance(new B2C.Message.SqlData.Query.QueryDaoBuilder(Infrastructure.SqldbType.mysql, () => configReader["message_conn"]));
                    x.RegisterInstance(new B2C.Message.SqlData.Repository.RepositoryDaoBuilder(Infrastructure.SqldbType.mysql, () => configReader["message_conn"]));
                });

            e.Startup.UseAutoInjectingAttributeUsingIoC(new IAutoInjectingEnvironmentProvider[]
            {
                //由于直接使用query与Repository，所以要全部注入admin与message的组件
                SingletonAutoInjectingEnvironmentProvider.UsingRuleContainerAutoInjectingEnvironmentProvider("admin"),
                SingletonAutoInjectingEnvironmentProvider.UsingRuleContainerAutoInjectingEnvironmentProvider("message"),
            })

            .UseConfigClient(new IPEndPoint(IPAddress.Parse(configReader["config_host"]), configReader.IntInAppConfig("config_port")), out var configFileClient);
            configFileClient.Startup(TimeSpan.FromMinutes(10), new[] { new ConfigFileClientRequest { FileName = "admin_web" } }, (c, t) =>
            {
                var content = t;
                if (c != null && c.FileName == "admin_web")
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(this.Environment.ContentRootPath, "appsettings.app.json"), content);
                }
            }).Push("admin_web").GetAwaiter().GetResult();

            e.Startup
                .UseCounterCache()
                .UseConcurrentCache()
                .UseDataContractJson()
                .UseEasyJson(string.Empty)
                .UseNLog(logfile)
                .UseMvcActionCustomRoute(e.Collector as IServiceCollection)
                .UseMvcModelStateValidation()
                .UseForceCheckAggregateRootImplIHandle()
                .UseForceCheckCommandAppDomainAttribute()
                .UseForceCheckCommandEvenWithNoParamaterCtor()
                .UseForceCheckCommandHandlerCtor()
                .UseForceCheckEventAppDomainAttribute()
                .UseForceCheckEventHandlerCtor()
                .UseForceCheckMessageSubscriberCtor()
                .UseAppConfig(configReader)
                .UseMvcPermission()
                .UseApiUriRouteDispatch(40, (x) => new IApiRouteProvider[]
                 {
                    new B2C.Message.Contract.Services.ApiRouteProvider(configReader),
                 }, () => e.Startup.ServiceLocator.ResolveOptional<ILoggerBuilder>())
                .UseHttpProxyGenerateMessageApi()
                .UseInjectingCommandHandlerEventHandler(Never.IoC.ComponentLifeStyle.Singleton)
                .UseSqliteEventProviderCommandBus<CommandContextWrapper>(new SqliteFailRecoveryStorager(commandfile, eventfile))
                .UseMvcDependency(e.Collector as IServiceCollection);

            //配置中心更新配置文件后，系统不一定马上能重新加载
            e.Startup.Startup(TimeSpan.FromSeconds(1), (x) =>
            {
                using (var sc = x.ServiceLocator.BeginLifetimeScope())
                {
                    var permission = sc.Resolve<IPermissionQuery>();
                    var auth = sc.Resolve<IAuthenticationService>();
                    sc.ResolveOptional<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
                    sc.ResolveOptional<B2C.Message.Contract.Services.IVCodeService>();
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
            services.AddMemoryCache();
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddMvc((x) =>
            {
                x.InputFormatters.Add(new JsonFormatter());
                x.OutputFormatters.Add(new JsonFormatter());
                x.Filters.Add(new SitemapFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = FormsAuthenticationService.FormsScheme;
                x.DefaultSignInScheme = FormsAuthenticationService.FormsScheme;
                x.DefaultScheme = FormsAuthenticationService.FormsScheme;
                x.DefaultChallengeScheme = FormsAuthenticationService.FormsScheme;
                x.DefaultForbidScheme = FormsAuthenticationService.FormsScheme;
                x.DefaultSignOutScheme = FormsAuthenticationService.FormsScheme;
            }).AddCookie(FormsAuthenticationService.FormsScheme, (x) =>
             {
                 x.LoginPath = "/Login/login";
                 x.Cookie.Path = "/";
                 x.LogoutPath = "/Login/logout";
                 x.ExpireTimeSpan = TimeSpan.FromDays(1);
             });

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

            app.UseSession();
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                CheckConsentNeeded = context => false,
                MinimumSameSitePolicy = SameSiteMode.None,
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areas", template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute("statarea", "stat", "{area:stat}/{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute("cmsarea", "cms", "{area:cms}/{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute("mediaarea", "media", "{area:media}/{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected static bool GetAssemblyName(string file)
        {
            if (file.IndexOf("B2C.Admin.Web.Views.dll", StringComparison.OrdinalIgnoreCase) > 0)
            {
                return false;
            }

            return true;
        }
    }
}