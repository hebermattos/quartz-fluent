using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Services
{
    public class SchedulerService
    {
        public async Task<IScheduler> Create(string instanceName, int threadCount)
        {
            NameValueCollection props = new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" },
                    {"quartz.scheduler.instanceName" , instanceName} ,
                    {"quartz.jobStore.type" , "Quartz.Simpl.RAMJobStore"} ,
                    {"quartz.threadPool.threadCount" , threadCount.ToString()}
                };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            IScheduler scheduler = await factory.GetScheduler();

            return scheduler;
        }
    }
}