using Never.Hosting.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace B2C.EasyConfig.Host
{
    /// <summary>
    /// 服务
    /// </summary>
    public static class Service
    {
        /// <summary>
        /// 服务
        /// </summary>
        private class HoseService : System.ServiceProcess.ServiceBase
        {
            private IHost host;

            public HoseService(IHost host)
            {
                this.host = host;
            }

            protected override void OnStart(string[] args)
            {
                this.host.Start();
                base.OnStart(args);

            }

            protected override void OnStop()
            {
                try
                {
                    this.host.StopAsync().GetAwaiter().GetResult();
                }
                finally
                {
                    this.host.Dispose();
                    base.OnStop();
                }
            }

        }

        /// <summary>
        /// 启动一个服务
        /// </summary>
        /// <param name="host"></param>
        public static void RunAsService(this IHost host)
        {
            var webHostService = new HoseService(host);
            System.ServiceProcess.ServiceBase.Run(webHostService);
        }
    }
}