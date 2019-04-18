using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Services
{
    public class SchedulerService
    {
        private IScheduler _scheduler;

        public async Task Create(string instanceName, int threadCount, ILogProvider logProvider)
        {
            LogProvider.SetCurrentLogProvider(logProvider);

            NameValueCollection props = new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" },
                    {"quartz.scheduler.instanceName" , instanceName} ,
                    {"quartz.jobStore.type" , "Quartz.Simpl.RAMJobStore"} ,
                    {"quartz.threadPool.threadCount" , threadCount.ToString()}
                };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            _scheduler = await factory.GetScheduler();
        }

        public Task Start()
        {
            return _scheduler.Start();
        }

        public Task ScheduleJob<T>(int intervalInSeconds) where T : IJob
        {
            IJobDetail job = JobBuilder.Create<T>()
                               .WithIdentity(typeof(T).Name, "job-group")
                               .Build();

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(typeof(T).Name, "trigger-group")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(intervalInSeconds)
                  .RepeatForever())
              .Build();

            return _scheduler.ScheduleJob(job, trigger);
        }

        public Task ScheduleJob<T>(string cronExpression) where T : IJob
        {
            IJobDetail job = JobBuilder.Create<T>()
                               .WithIdentity(typeof(T).Name, "job-group")
                               .Build();

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(typeof(T).Name, "trigger-group")
              .StartNow()
              .WithCronSchedule(cronExpression)
              .Build();

            return _scheduler.ScheduleJob(job, trigger);
        }
    }
}