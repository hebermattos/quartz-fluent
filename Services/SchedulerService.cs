using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Services
{
    public class SchedulerService
    {
        public async Task<IScheduler> Create()
        {
            NameValueCollection props = new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" },
                    {"quartz.scheduler.instanceName" , "MyScheduler"} ,
                    {"quartz.jobStore.type" , "Quartz.Simpl.RAMJobStore"} ,
                    {"quartz.threadPool.threadCount" , "3"}
                };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            IScheduler scheduler = await factory.GetScheduler();

            return scheduler;
        }
    }
}