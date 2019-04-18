using Quartz;

namespace Services
{
    public class JobService
    {
        public (IJobDetail, ITrigger) GetJobConfig<T>(int interval) where T : IJob
        {
            IJobDetail job = JobBuilder.Create<T>()
                             .WithIdentity(typeof(T).Name, "job-group")
                             .Build();

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(typeof(T).Name, "trigger-group")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(interval)
                  .RepeatForever())
              .Build();

            return (job, trigger);
        }
    }
}