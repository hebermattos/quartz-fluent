using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;

namespace Services
{
    public class SchedulerService : ISchedulerService
    {
        private List<(IJobDetail, ITrigger)> _jobs;

        private IScheduler _quartzScheduler;

        public SchedulerService(IScheduler quartzScheduler)
        {
            _quartzScheduler = quartzScheduler;

            _jobs = new List<(IJobDetail, ITrigger)>();
        }

        public ISchedulerService ScheduleJob<T>(int intervalInSeconds) where T : IJob
        {
            IJobDetail job = Createjob<T>();

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(typeof(T).Name, "trigger-group")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(intervalInSeconds)
                  .RepeatForever())
              .Build();

            _jobs.Add((job, trigger));

            return this;
        }

        public ISchedulerService ScheduleJob<T>(string cronExpression) where T : IJob
        {
            IJobDetail job = Createjob<T>();

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(typeof(T).Name, "trigger-group")
              .StartNow()
              .WithCronSchedule(cronExpression)
              .Build();

            _jobs.Add((job, trigger));

            return this;
        }

        public async Task Run()
        {            
            await _quartzScheduler.Start();

            foreach (var job in _jobs)
                await _quartzScheduler.ScheduleJob(job.Item1, job.Item2);
        }

        private static IJobDetail Createjob<T>() where T : IJob
        {
            IJobDetail job = JobBuilder.Create<T>()
                               .WithIdentity(typeof(T).Name, "job-group")
                               .Build();
            return job;
        }
    }
}