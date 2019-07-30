using System;
using Quartz;
using Quartz.Spi;

namespace Scheduler
{
    public class IoCJobFactory : IJobFactory
    {
        protected readonly IServiceProvider Container;

        public IoCJobFactory(IServiceProvider container)
        {
            Container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = Container.GetService(bundle.JobDetail.JobType) as IJob;

            return job;
        }

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
    }
}