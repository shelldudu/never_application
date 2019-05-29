using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Never;
using Never.Commands;
using Never.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                switch (args[0])
                {
                    default:
                        {
                            CreateWebHostBuilderRuningConsole(args).Build().Run();
                        }
                        break;

                    case "src":
                    case "svc":
                    case "srv":
                        {
                            CreateWebHostBuilderRuningService(args).Build().RunAsService();
                            break;
                        }
                }
                return;
            }

            CreateWebHostBuilderRuningConsole(args).Build().Run();
            return;
        }

        public static IWebHostBuilder CreateWebHostBuilderRuningConsole(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                  .UseStartup<Startup>()
                  .UseJsonFileConfig(Never.Web.Mvc.StartupExtension.ConfigFileBuilder(new[] { "appsettings.json", "appsettings.app.json" }))
                  .UseKestrel((builder,option) =>
                  {
                      var ports = string.Empty;
                      try
                      {
                         ports = builder.Configuration.GetValue<string>("server.ports");
                      }
                      catch
                      {
                          return;
                      }

                      if (ports.IsNullOrEmpty())
                          return;

                      foreach (var split in ports.Split(new char[] { ',', ';', ':' }).Select(t => t.AsInt()).Distinct())
                      {
                         option.Listen(System.Net.IPAddress.Any, split);
                      }
                  });
        }

        public static IWebHostBuilder CreateWebHostBuilderRuningService(string[] args)
        {
            var pathToExe = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            return WebHost.CreateDefaultBuilder(args)
                .UseJsonFileConfig(Never.Web.Mvc.StartupExtension.ConfigFileBuilder(new[] { "appsettings.json", "appsettings.app.json" }))
                .UseKestrel((builder,option) =>
                {
                    var ports = string.Empty;
                    try
                    {
                       ports = builder.Configuration.GetValue<string>("server.ports");
                    }
                    catch
                    {
                        return;
                    }

                    if (ports.IsNullOrEmpty())
                        return;

                    foreach (var split in ports.Split(new char[] { ',', ';', ':' }).Select(t => t.AsInt()).Distinct())
                    {
                       option.Listen(System.Net.IPAddress.Any, split);
                    }
                })
            .UseContentRoot(pathToContentRoot)
            .UseStartup<Startup>();
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public static int PageSize
        {
            get
            {
                return 15;
            }
        }

        /// <summary>
        /// 获取Model的错误信息
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string ModelErrorMessage(this Microsoft.AspNetCore.Mvc.Razor.RazorPage page)
        {
            return ModelErrorMessage(page.ViewContext);
        }

        /// <summary>
        /// 获取Model的错误信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static string ModelErrorMessage(this Microsoft.AspNetCore.Mvc.ActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid)
                return string.Empty;

            if (actionContext.ModelState.Any() && actionContext.ModelState.Keys.Any())
            {
                foreach (var key in actionContext.ModelState.Keys)
                {
                    var errors = actionContext.ModelState[key];
                    if (errors != null && errors.Errors != null && errors.Errors.Any())
                    {
                        foreach (var e in errors.Errors)
                        {
                            return e.ErrorMessage;
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// model验证错误信息
        /// </summary>
        public static IEnumerable<Never.Web.Mvc.Controllers.BasicController.ModelStateError> ModelError(this Microsoft.AspNetCore.Mvc.Razor.RazorPage page)
        {
            if (page.TempData["ModelErrorKey"] != null)
                return (IEnumerable<Never.Web.Mvc.Controllers.BasicController.ModelStateError>)page.TempData["ModelErrorKey"];

            var list = ModelError(page.ViewContext);
            if (list != null)
                page.TempData["ModelErrorKey"] = list.ToList();
            else
                page.TempData["ModelErrorKey"] = new Never.Web.Mvc.Controllers.BasicController.ModelStateError[0];

            return list;
        }

        /// <summary>
        /// model验证错误信息
        /// </summary>
        public static IEnumerable<Never.Web.Mvc.Controllers.BasicController.ModelStateError> ModelError(this Microsoft.AspNetCore.Mvc.ActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid)
                yield break;

            if (actionContext.ModelState.Any() && actionContext.ModelState.Keys.Any())
            {
                foreach (var key in actionContext.ModelState.Keys)
                {
                    var errors = actionContext.ModelState[key];
                    if (errors != null && errors.Errors != null && errors.Errors.Any())
                    {
                        foreach (var e in errors.Errors)
                        {
                            yield return new Never.Web.Mvc.Controllers.BasicController.ModelStateError()
                            {
                                Exception = e.Exception,
                                Message = e.ErrorMessage,
                                MemberName = key
                            };
                        }
                    }
                }
            }

            yield break;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static HtmlString Content(this Microsoft.AspNetCore.Mvc.Razor.RazorPage page ,string path, params object[] args)
        {
            string contentPath = string.Format("~/" + path.Trim(new char[]
            {
                '/',
                '~',
                '\\',
                '.'
            }), args);
            return new HtmlString(new UrlHelper(page.ViewContext).Content(contentPath));
        }
    }
}
