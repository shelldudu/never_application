using Microsoft.Extensions.Hosting;
using Never.Hosting;
using System;
using System.IO;

namespace B2C.EasyConfig.Host
{
    class Program
    {
        static void Main(string[] args)
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

        public static IHostBuilder CreateWebHostBuilderRuningConsole(string[] args)
        {
            return new HostBuilder()
                  .UseStartup<Startup>(HostingExtension.ConfigFileBuilder(new[] { "appsettings.json" }));
        }

        public static IHostBuilder CreateWebHostBuilderRuningService(string[] args)
        {
            var pathToExe = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            return new HostBuilder()
                .UseStartup<Startup>(HostingExtension.ConfigFileBuilder(new[] { "appsettings.json" }))
                .UseContentRoot(pathToContentRoot);
        }
    }
}
