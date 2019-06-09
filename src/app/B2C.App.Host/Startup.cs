using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Never;
using Never.Caching;
using Never.Commands;
using Never.Configuration;
using Never.Configuration.ConfigCenter;
using Never.Configuration.ConfigCenter.Remoting;
using Never.Deployment;
using Never.IoC;
using Never.Logging;
using Never.NLog;
using Never.Serialization;
using Never.Serialization.Json;
using Never.Utils;
using Never.Web;
using Never.Web.Encryptions;
using Never.Web.WebApi;
using Never.Web.WebApi.Encryptions;

namespace B2C.App.Host
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
                    x.RegisterType<AuthenticationService, AuthenticationService>(string.Empty, ComponentLifeStyle.Singleton);
                    x.RegisterType<ProxyRouteDispatcher, ProxyRouteDispatcher>(string.Empty, ComponentLifeStyle.Singleton);
                    x.RegisterType<ProxyMiddlewear, ProxyMiddlewear>(string.Empty, ComponentLifeStyle.Singleton);
                },
                (x, y, z) =>
                {
                });

            e.Startup.UseConfigClient(new IPEndPoint(IPAddress.Parse(configReader["config_host"]), configReader.IntInAppConfig("config_port")), out var configFileClient);
            configFileClient.Startup(TimeSpan.FromMinutes(10), new[] { new ConfigFileClientRequest { FileName = "app_host" } }, (c, t) =>
            {
                var content = t;
                if (c != null && c.FileName == "app_host")
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(this.Environment.ContentRootPath, "appsettings.app.json"), content);
                }
            }).Push("app_host").GetAwaiter().GetResult();

            e.Startup
                .UseCounterCache()
                .UseConcurrentCache()
                .UseNLog(logfile)
                .UseEasyJson(string.Empty)
                .UseApiModelStateValidation()
                .UseAppConfig(configReader)
                .UseApiActionCustomRoute(e.Collector as IServiceCollection)
                .UseApiDependency(e.Collector as IServiceCollection);

            //配置中心更新配置文件后，系统不一定马上能重新加载
            e.Startup.Startup(TimeSpan.FromSeconds(1), (x) =>
            {
                using (var sc = x.ServiceLocator.BeginLifetimeScope())
                {
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return base.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app)
        {
            if (this.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<ProxyMiddlewear>();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
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
            if (file.IndexOf("B2C.App.Host.Views.dll", StringComparison.OrdinalIgnoreCase) > 0)
            {
                return false;
            }

            return true;
        }

        private class ProxyMiddlewear : IMiddleware
        {
            private readonly AuthenticationService authenticationService = null;
            private readonly ApiUriDispatcher<IApiRouteProvider> proxyRouteDispatcher = null;

            public ProxyMiddlewear(AuthenticationService authenticationService, IConfigReader configReader)
            {
                this.authenticationService = authenticationService;
                var provider = new ProxyRouteDispatcher(configReader);
                var a10 = Never.Deployment.StartupExtension.StartReport().Startup(60, new[] { provider });
                this.proxyRouteDispatcher = new ApiUriDispatcher<IApiRouteProvider>(provider, a10);
            }

            public Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                if (!context.Request.Path.HasValue)
                {
                    return next(context);
                }

                if (!context.Request.Method.Equals("post", System.StringComparison.OrdinalIgnoreCase))
                {
                    return next(context);
                }

                var segments = context.Request.Path.Value.ToLower().Split('/', System.StringSplitOptions.RemoveEmptyEntries);
                if (segments != null && segments.Length == 3 && segments[0] == "api")
                {
                    //拿api地址
                    var host = new HostString(this.proxyRouteDispatcher.GetCurrentUrlHost((context.Request.ContentLength.HasValue ? context.Request.ContentLength.Value : segments[1].GetHashCode()).ToString()));
                    var url = UriHelper.BuildAbsolute("http", host, context.Request.PathBase, context.Request.Path, context.Request.QueryString, default(FragmentString));

                    var token = this.authenticationService.GetToken(context);
                    IContentEncryptor enctryptor = new TripleDESContentEncryptor(token.CryptToken);
                    var header = this.ConvertHeaders(context, token);
                    //using (var body = this.ConvertContentFromBodyString(context, enctryptor))
                    using (var body = this.ConvertContentFromBodyByteArray(context, enctryptor))
                    {
                        //注册与登陆，由于在这里做identity servie
                        switch (segments[2])
                        {
                            //注册
                            case "Register":
                            //登陆
                            case "Login":
                                {
                                    var loginTask = new WebRequestDownloader().PostString(new Uri(url), body, header, "application/json", 0);
                                    var loginContent = loginTask;
                                    var target = EasyJsonSerializer.Deserialize<Never.Web.WebApi.Controllers.BasicController.ResponseResult<UserIdToken>>(loginContent);
                                    if (target != null && target.Code == "0000" && target.Data.UserId > 0)
                                    {
                                        var token2 = this.authenticationService.SignIn(context, target.Data.UserId).GetAwaiter().GetResult();
                                        var appresult = new Never.Web.WebApi.Controllers.BasicController.ResponseResult<AppToken>(target.Code, new AppToken { @accesstoken = token2.AccessToken }, target.Message);
                                        return this.ConvertContentToBody(context, EasyJsonSerializer.Serialize(appresult), enctryptor);
                                    }

                                    var appresult2 = new Never.Web.WebApi.Controllers.BasicController.ResponseResult<AppToken>(target.Code, new AppToken { @accesstoken = string.Empty }, target.Message);
                                    return this.ConvertContentToBody(context, EasyJsonSerializer.Serialize(appresult2), enctryptor);
                                }
                            //退出
                            case "Logout":
                                {
                                    new WebRequestDownloader().PostString(new Uri(url), body, header, "application/json", 0);
                                    this.authenticationService.SignOut(context, token);
                                    var appresult = new Never.Web.WebApi.Controllers.BasicController.ResponseResult<AppToken>("0000", new AppToken { @accesstoken = string.Empty }, string.Empty);
                                    return this.ConvertContentToBody(context, EasyJsonSerializer.Serialize(appresult), enctryptor);
                                }
                        }

                        if (header.ContainsKey("userid"))
                        {
                            url = string.Concat(url, "?userid=", header["userid"]);
                        }

                        var task = new WebRequestDownloader().Post(new Uri(url), body, header, "application/json", 0);
                        return this.ConvertContentToBody(context, task, enctryptor);
                        //    var logger = Never.IoC.ContainerContext.Current.ServiceLocator.Resolve<ILoggerBuilder>().Build(typeof(Startup));
                        //    var action = new Action<string>((x) =>
                        //    {
                        //        logger.Warn(url + ":" + x);
                        //    });

                        //    using (var method = new Never.Utils.MethodTickCount(action))
                        //    {
                        //        var task = new WebRequestDownloader().PostString(new Uri(url), body, header, "application/json");
                        //        var content = task;// task.GetAwaiter().GetResult();
                        //        return context.Response.WriteAsync(this.ConvertResult(content, token));
                        //    }
                    }
                }

                return next(context);
            }

            private IDictionary<string, string> ConvertHeaders(HttpContext context, Token token)
            {
                var headers = new Dictionary<string, string>();
                if (context.Connection.RemoteIpAddress != null)
                {
                    headers["ip"] = context.Connection.RemoteIpAddress.ToString();
                }
                if (context.Request.Headers != null)
                {
                    if (context.Request.Headers.ContainsKey("X-Real-IP"))
                    {
                        var ip = context.Request.Headers["X-Real-IP"].FirstOrDefault();
                        if (ip.IsIP())
                            headers["ip"] = ip;
                    }
                    else if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                    {
                        var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                        if (ip.IsIP())
                            headers["ip"] = ip;
                    }
                }

                var user = this.authenticationService.GetUser(context, token);
                if (user.HasValue && user.Value > 0)
                {
                    headers["userid"] = user.Value.ToString();
                }

                if (context.Request.Headers != null && context.Request.Headers.Any())
                {
                    foreach (var key in context.Request.Headers.Keys)
                    {
                        switch (key)
                        {
                            case "platform":
                                {
                                    var value = context.Request.Headers[key];
                                    headers[key] = value.ToString();
                                }
                                break;
                        }
                    }
                }

                return headers;
            }

            private Stream ConvertContentFromBodyString(HttpContext context, IContentEncryptor enctryptor)
            {
                using (var st = new StreamReader(context.Request.Body))
                {
                    var writer = new StreamWriter(new MemoryStream());
                    var content = st.ReadToEnd();
                    if (content.IsNullOrWhiteSpace())
                    {
                        writer.Write("");
                    }
                    else
                    {
                        writer.Write(enctryptor.Decrypt(content));
                    }

                    writer.Flush();
                    return writer.BaseStream;
                }
            }

            private Stream ConvertContentFromBodyByteArray(HttpContext context, IContentEncryptor enctryptor)
            {
                using (var st = new MemoryStream())
                {
                    context.Request.Body.CopyTo(st);
                    st.Position = 0;
                    var @byte = st.ToArray();
                    return enctryptor.Decrypt(@byte, new[] { "utf-8" });
                }
            }

            private Task ConvertContentToBody(HttpContext context, byte[] content, IContentEncryptor enctryptor)
            {
                var @byte = enctryptor.Encrypt(content);
                return context.Response.Body.WriteAsync(@byte, 0, @byte.Length);
            }

            private Task ConvertContentToBody(HttpContext context, string content, IContentEncryptor enctryptor)
            {
                var @string = enctryptor.Encrypt(content);
                return context.Response.WriteAsync(@string);
            }
        }

        private class ProxyRouteDispatcher : DefaultApiRouteProvider
        {
            private readonly IConfigReader configReader = null;
            public ProxyRouteDispatcher(IConfigReader configReader)
            {
                this.configReader = configReader;

            }
            public override IEnumerable<ApiUrlA10Element> ApiUrlA10Elements
            {
                get
                {
                    var pings = new List<string>();
                    var urls = new List<string>();
                    for (var i = 0; i <= 100; i++)
                    {
                        var url = this.configReader[string.Concat("AppA10:url:", i.ToString())];
                        if (string.IsNullOrWhiteSpace(url))
                        {
                            break;
                        }

                        var ping = this.configReader[string.Concat("AppA10:ping:", i.ToString())];
                        if (string.IsNullOrWhiteSpace(ping))
                        {
                            throw new System.Exception("ping与url条数不一致");
                        }

                        urls.Add(string.Concat(url.Trim().TrimEnd('/'), '/'));
                        pings.Add(ping.Trim());
                    }

                    for (var i = 0; i < urls.Count; i++)
                    {
                        var uri = new Uri(urls[i]);
                        yield return new ApiUrlA10Element()
                        {
                            ApiUrl = string.Concat(uri.Host, ":", uri.Port),
                            A10Url = pings[i]
                        };
                    }
                }
            }
        }

        private class AuthenticationService
        {
            private readonly ConcurrentCounterDictCache<string, long> concurrentCounter = null;
            private readonly ConcurrentCounterDictCache<long, string> singleCounter = null;
            #region ctor

            /// <summary>
            ///
            /// </summary>
            /// <param name="httpContext"></param>
            public AuthenticationService()
            {
                this.concurrentCounter = new ConcurrentCounterDictCache<string, long>(3000);
                this.singleCounter = new ConcurrentCounterDictCache<long, string>(3000);
            }

            #endregion ctor

            #region IAuthenticationService成员

            public Token GetToken(HttpContext context)
            {
                var token = context.Request.Headers.ContainsKey("accesstoken") ? context.Request.Headers["accesstoken"].FirstOrDefault() : string.Empty;
                if (string.IsNullOrEmpty(token))
                {
                    return new Token() { CryptToken = "56dc54a07f3d15a400000155" };
                }

                try
                {
                    var splits = token.From3DES("56dc54a07f3d15a400000155").Split('|');
                    if (splits != null && splits.Length == 2)
                    {
                        return new Token()
                        {
                            AccessToken = token,
                            CryptToken = splits[0],
                            UserToken = splits[1]
                        };
                    }
                }
                catch
                {
                    return new Token() { CryptToken = "56dc54a07f3d15a400000155" };
                }

                return new Token() { CryptToken = "56dc54a07f3d15a400000155" };
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="user"></param>
            public virtual Task<Token> SignIn(HttpContext context, long user)
            {
                var cryptToke = NewId.GenerateString(NewId.StringLength.L24);
                var userToken = NewId.GenerateGuid().ToString().Replace("-", "");
                var accessToken = string.Concat(cryptToke, "|", userToken).To3DES("56dc54a07f3d15a400000155");
                var token = new Token()
                {
                    CryptToken = cryptToke,
                    UserToken = userToken,
                    AccessToken = accessToken
                };

                var oldtoken = this.singleCounter.GetValue(user);
                if (oldtoken.IsNotNullOrEmpty())
                {
                    this.concurrentCounter.RemoveValue(oldtoken);
                }

                this.concurrentCounter.SetValue(token.UserToken, user, TimeSpan.FromDays(7));
                this.singleCounter.SetValue(user, token.UserToken);
                return Task.FromResult(token);
            }

            /// <summary>
            ///
            /// </summary>
            public virtual Task SignOut(HttpContext context, Token token)
            {
                if (token.UserToken.IsNotNullOrEmpty())
                {
                    if (this.concurrentCounter.TryRemoveValue(token.UserToken, out var user))
                    {
                        this.singleCounter.RemoveValue(user);
                    }
                }

                return Task.CompletedTask;
            }

            /// <summary>
            /// 获取会员
            /// </summary>
            /// <returns></returns>
            public long? GetUser(HttpContext context, Token token)
            {
                if (token.UserToken.IsNullOrEmpty())
                {
                    return null;
                }

                return this.concurrentCounter.GetValue(token.UserToken);
            }

            #endregion IAuthenticationService成员
        }

        private struct Token
        {
            public string AccessToken { get; set; }
            public string CryptToken { get; set; }
            public string UserToken { get; set; }
        }

        private struct AppToken
        {
            public string accesstoken { get; set; }
        }

        private struct UserIdToken
        {
            public long UserId { get; set; }
        }
    }
}