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
using Never.Caching;
using Never.Configuration;
using Never.Configuration.ConfigCenter;
using Never.Configuration.ConfigCenter.Remoting;
using Never.Deployment;
using Never.IoC;
using Never.Logging;
using Never.NLog;
using Never.Serialization;
using Never.Serialization.Json;
using Never.Web.WebApi;
using System;
using System.IO;
using System.Net;

namespace B2C.App.Api
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

            DefaultSetting.DefaultDeserializeSetting = new JsonDeserializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };
            DefaultSetting.DefaultSerializeSetting = new JsonSerializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };

            e.Startup.RegisterAssemblyFilter("B2C".CreateAssemblyFilter()).RegisterAssemblyFilter("Never".CreateAssemblyFilter());
            e.Startup.UseEasyIoC(
                (x, y, z) =>
                {
                    var mockTest = new Never.Aop.DynamicProxy.Mock<IUserService>();
                    mockTest.Setup(m => m.GetUser(0)).Return(m =>
                    {
                        return new ApiResult<UserModel>() { Status = ApiStatus.Success, Result = new UserModel() { UserId = 2233, Mobile = "13800138000" }, Message = "666" };
                    });
                    mockTest.Setup(m => m.GetCount("")).Return(m =>
                    {
                        return new ApiResult<int>(ApiStatus.Success, 0);
                    });
                    mockTest.Setup(m => m.LockUser(0)).Return(m =>
                    {
                        return new ApiResult<string>(ApiStatus.Fail, string.Empty, "找不到用户");
                    });
                    mockTest.Setup(m => m.UnlockUser(0)).Return(m =>
                    {
                        return new ApiResult<string>(ApiStatus.Fail, string.Empty, "找不到用户");
                    });
                    mockTest.Setup(m => m.Login(new LoginUserReqs())).Return(m =>
                    {
                        return new ApiResult<UserModel>() { Status = ApiStatus.Success, Result = new UserModel() { UserId = 2233, Mobile = "13800138000" }, Message = "666" };
                    });
                    mockTest.Setup(m => m.Register(new RegisterUserReqs())).Return(m =>
                    {
                        return new ApiResult<UserModel>() { Status = ApiStatus.Success, Result = new UserModel() { UserId = 2233, Mobile = "13800138000" }, Message = "666" };
                    });
                    mockTest.Setup(m => m.ChangePassword(new ChangePwdReqs())).Return(m =>
                    {
                        return new ApiResult<string>(ApiStatus.Fail, string.Empty, "找不到用户");
                    });

                    x.RegisterInstance(mockTest.CreateIllusive(), typeof(IUserService));
                    x.RegisterType<UserProxyService, IUserProxyService>().WithParameter<ICaching>("ConcurrentCache").AsProxy().WithInterceptor<Never.Aop.IInterceptors.StopwatchInterceptor>("stop");
                },
                (x, y, z) =>
                {
                });

            e.Startup.UseAutoInjectingAttributeUsingIoC(new IAutoInjectingEnvironmentProvider[]
            {

            })
            .UseConfigClient(new IPEndPoint(IPAddress.Parse(configReader["config_host"]), configReader.IntInAppConfig("config_port")), out var configFileClient);
            configFileClient.Startup(TimeSpan.FromMinutes(10), new[] { new ConfigFileClientRequest { FileName = "app_api" } }, (c, t) =>
            {
                var content = t;
                if (c != null && c.FileName == "app_api")
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(this.Environment.ContentRootPath, "appsettings.app.json"), content);
                }
            }).Push("app_api").GetAwaiter().GetResult();

            e.Startup
                .UseCounterCache()
                .UseConcurrentCache("ConcurrentCache")
                .UseDataContractJson()
                .UseNLog(logfile)
                .UseEasyJson(string.Empty)
                .UseForceCheckAggregateRootImplIHandle()
                .UseForceCheckCommandAppDomainAttribute()
                .UseForceCheckCommandEvenWithNoParamaterCtor()
                .UseForceCheckCommandHandlerCtor()
                .UseForceCheckEventAppDomainAttribute()
                .UseForceCheckEventHandlerCtor()
                .UseForceCheckMessageSubscriberCtor()
                .UseInjectingCommandHandlerEventHandler(Never.IoC.ComponentLifeStyle.Singleton)
                .UseApiModelStateValidation()
                .UseApiUriRouteDispatch(40, (x) => new IApiRouteProvider[]
                 {
                    new B2C.Message.Contract.Services.ApiRouteProvider(configReader),
                 }, () => e.Startup.ServiceLocator.ResolveOptional<ILoggerBuilder>())
                .UseHttpProxyGenerateMessageApi()
                .UseAppConfig(configReader)
                .UseApiActionCustomRoute(e.Collector as IServiceCollection)
                .UseApiDependency(e.Collector as IServiceCollection);

            //配置中心更新配置文件后，系统不一定马上能重新加载
            e.Startup.Startup(TimeSpan.FromSeconds(1), (x) =>
            {
                using (var sc = x.ServiceLocator.BeginLifetimeScope())
                {
                    var serv = sc.Resolve<IUserService>();
                    sc.Resolve<IVCodeService>();
                    sc.Resolve<IUserService>();
                    sc.Resolve<IUserProxyService>();
                    sc.Resolve<Controllers.LoginController>();
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
            var exceptionhandler = new ExceptionHandlerOptions()
            {
                ExceptionHandler = (context) =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "application/json";
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null && feature.Error != null)
                    {
                        var loggerbuilder = context.RequestServices.GetServiceOptional<ILoggerBuilder>();
                        var logger = loggerbuilder.Build(typeof(Startup));
                        var url = UriHelper.BuildRelative(context.Request.PathBase, context.Request.Path, context.Request.QueryString, default(FragmentString));
                        logger.Error(string.Format("请求接口报错：{0}", url), feature.Error);
                    }

                    return context.Response.WriteAsync(SerializeEnvironment.JsonSerializer.Serialize(new Never.Web.WebApi.Controllers.BasicController.ResponseResult<string>("0001", string.Empty, "服务器繁忙，请稍后再试")));
                }
            };
            app.UseExceptionHandler(exceptionhandler);

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Get}/{id?}");
            });

            app.UseStaticFiles();
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected static bool GetAssemblyName(string file)
        {
            if (file.IndexOf("B2C.App.Api.Views.dll", StringComparison.OrdinalIgnoreCase) > 0)
            {
                return false;
            }

            return true;
        }
    }
}