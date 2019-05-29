using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Never;
using Never.Web.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace B2C.App.Api
{
    public class Program
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
                  .UseJsonFileConfig(Never.Web.WebApi.StartupExtension.ConfigFileBuilder(new[] { "appsettings.json", "appsettings.app.json" }))
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
                .UseJsonFileConfig(Never.Web.WebApi.StartupExtension.ConfigFileBuilder(new[] { "appsettings.json", "appsettings.app.json" }))
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
    }
}