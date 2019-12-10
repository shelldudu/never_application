using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Never;
using Never.Commands;
using Never.Configuration;
using Never.IoC;
using Never.Logging;
using Never.NLog;
using Never.Serialization;
using Never.Serialization.Json;
using Never.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Never.Configuration.ConfigCenter;
using Never.Configuration.ConfigCenter.Remoting;

namespace B2C.EasyConfig.Host
{
    public class Startup : Never.Hosting.AppStartup, ICustomKeyValueFinder
    {
        public Startup(IConfiguration configuration,
            Microsoft.Extensions.Hosting.IHostingEnvironment environment) : base(() => new Never.ApplicationStartup(new Never.IoC.Providers.AppDomainAssemblyProvider(GetAssemblyName)))
        {
            this.Configuration = configuration;
            this.Environment = environment;
            this.OnStarting += this.Startup_OnStarting;
        }

        private void Startup_OnStarting(object sender, Never.StartupEventArgs e)
        {
            var logfile = new FileInfo(AppContext.BaseDirectory + "\\App_Config\\nlog.config");
            var configReader = new AppConfigReader(this.Configuration);

            //配置文件
            var listenPoint = new IPEndPoint(IPAddress.Parse(configReader["socket_host"]), configReader.IntInAppConfig("socket_port"));
            var shareFileEventHandler = new EventHandler<ShareFileEventArgs>((ss, ee) => { });
            e.Startup.UseJsonConfigServer(null, shareFileEventHandler, null, this, listenPoint, out var configFileServer, out var configurationWatcher);
            configurationWatcher.AddShareFile(System.IO.Path.Combine(AppContext.BaseDirectory, "App_Data", "share"), "*.json", Encoding.UTF8, null);
            configurationWatcher.AddAppFile(System.IO.Path.Combine(AppContext.BaseDirectory, "App_Data", "app"), "*.json", Encoding.UTF8);
            configurationWatcher.LoggerBuilder = () => e.Startup.ServiceLocator.Resolve<ILoggerBuilder>();

            //配置文件
            //var allFiles = System.IO.Directory.GetFiles(System.IO.Path.Combine(AppContext.BaseDirectory, "App_Data"), "*.json").Select(ta => new FileInfo(ta));
            //var shareConfig = configReader["share_config"].Split(new[] { ';', '|', ',' }).Distinct();
            //var shareFiles = allFiles.Where(ta => shareConfig.Contains(ta.Name)).Select(ta => new ConfigFileInfo() { File = ta, Encoding = Encoding.UTF8 }).ToList();
            //var appFiles = allFiles.Where(ta => !shareConfig.Contains(ta.Name)).ToList().Select(ta => new ConfigFileInfo() { File = ta, Encoding = Encoding.UTF8 }).ToList();
            //e.Startup.UseJsonConfigServer(shareFiles, shareFileEventHandler, appFiles, this, listenPoint, out var configFileServer, out var configurationWatcher);

            /*json序列化配置*/
            DefaultSetting.DefaultDeserializeSetting = new JsonDeserializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };
            DefaultSetting.DefaultSerializeSetting = new JsonSerializeSetting() { DateTimeFormat = DateTimeFormat.ChineseStyle };

            e.Startup.RegisterAssemblyFilter("B2C".CreateAssemblyFilter()).RegisterAssemblyFilter("Never".CreateAssemblyFilter());
            e.Startup.UseEasyIoC(
                (x, y, z) =>
                {
                },
                (x, y, z) =>
                {
                });

            e.Startup.UseAutoInjectingAttributeUsingIoC(new IAutoInjectingEnvironmentProvider[]
            {

            });
            e.Startup
                .UseCounterCache()
                .UseConcurrentCache()
                .UseDataContractJson()
                .UseEasyJson(string.Empty)
                .UseNLog(logfile)
                .UseAppConfig(this.Configuration)
                .UseForceCheckAggregateRootImplIHandle()
                .UseForceCheckCommandAppDomainAttribute()
                .UseForceCheckCommandEvenWithNoParamaterCtor()
                .UseForceCheckCommandHandlerCtor()
                .UseForceCheckEventAppDomainAttribute()
                .UseForceCheckEventHandlerCtor()
                .UseForceCheckMessageSubscriberCtor()
                .UseInjectingCommandHandlerEventHandler(Never.IoC.ComponentLifeStyle.Singleton)
                .UseInprocEventProviderCommandBus<DefaultCommandContext>()
                .UseHostingDependency(e.Collector as IServiceCollection);

            e.Startup.Startup(TimeSpan.FromSeconds(2), (x) =>
            {
                using (var sc = x.ServiceLocator.BeginLifetimeScope())
                {
                    sc.Resolve<ICommandBus>();
                    sc.Resolve<ILoggerBuilder>();
                    sc.Resolve<IJsonSerializer>();

                    //开始监听
                    configFileServer.Startup();
                    var logger = sc.Resolve<ILoggerBuilder>().Build(typeof(Startup));
                    logger.Info(string.Concat("startup at ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    logger.Info(string.Concat("socket listening at ", listenPoint.Address.ToString(), ":", listenPoint.Port));
                    Console.WriteLine(string.Concat("socket listening at ", listenPoint.Address.ToString(), ":", listenPoint.Port));
                }

            });
        }

        public IConfiguration Configuration { get; }

        public Microsoft.Extensions.Hosting.IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddMvc((x) =>
            //{
            //    x.InputFormatters.Add(new JsonFormatter());
            //    x.OutputFormatters.Add(new JsonFormatter());
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return base.ConfigureServices(services);
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected static bool GetAssemblyName(string file)
        {
            if (file.IndexOf("B2C.EasyConfig.Host.Views.dll", StringComparison.OrdinalIgnoreCase) > 0)
            {
                return false;
            }

            return true;
        }

        public string Find(string mode, string key)
        {
            switch (key)
            {
                case "user":
                    {
                        return "aryoueok";
                    }
            }

            return key;
        }
    }
}