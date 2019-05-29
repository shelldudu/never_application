using Never.Configuration;
using Never.Logging;
using Never.QuartzNET;
using Never.Threading;
using Quartz;
using System;
using System.Threading.Tasks;

namespace B2C.Admin.Taskschd.Tiggers
{
    [JobDescriptor(CronSchedule = "1/5 * * * * ?", Name = "测试作业", Descn = "每5秒查询一次测试作业跑", Heartbeat = 5 * 60 * 1000)]
    public class Worker : IJob
    {
        #region field and ctor
        private readonly ILoggerBuilder loggerBuilder = null;
        private readonly IConfigReader configReader = null;
        private readonly ITest test = null;
        private static readonly IRigidLocker locker = new MonitorLocker();

        public Worker(ILoggerBuilder loggerBuilder, IConfigReader configReader, ITest test)
        {
            this.loggerBuilder = loggerBuilder;
            this.configReader = configReader;
            this.test = test;
        }

        #endregion

        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask.ContinueWith(ta =>
            {
                locker.EnterLock(true, () =>
                {
                    this.test.Write(DateTime.Now);
                });
            });
        }
    }
}
