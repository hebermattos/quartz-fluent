using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Services
{
    public class SchedulerService : IScheduler
    {
        private List<(IJobDetail, ITrigger)> _jobs;

        private NameValueCollection _quartzProps;

        public IScheduler CreateInMemoryScheduler(ILogProvider logProvider = null)
        {
            _jobs = new List<(IJobDetail, ITrigger)>();

            if (logProvider != null)
                LogProvider.SetCurrentLogProvider(logProvider);

            _quartzProps = new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" },
                    {"quartz.scheduler.instanceName" , Guid.NewGuid().ToString()} ,
                    {"quartz.jobStore.type" , "Quartz.Simpl.RAMJobStore, Quartz"}
                };

            return this;
        }

        public IScheduler CreateSqlServerScheduler(string connectionString, ILogProvider logProvider = null)
        {
            _jobs = new List<(IJobDetail, ITrigger)>();

            if (logProvider != null)
                LogProvider.SetCurrentLogProvider(logProvider);

            _quartzProps = new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" },
                    {"quartz.scheduler.instanceName" , Guid.NewGuid().ToString()} ,
                    {"quartz.jobStore.type" , "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz"},
                    {"quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.StdAdoDelegate"},
                    {"quartz.jobStore.tablePrefix", "QRTZ_"},
                    {"quartz.jobStore.dataSource", "default"},
                    {"quartz.dataSource.default.connectionString", connectionString},
                    {"quartz.dataSource.default.provider", "SqlServer"},
                    {"quartz.jobStore.useProperties", "true"},
                    {"quartz.jobStore.lockHandler.type", "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz"}
                };

            return this;
        }

        public IScheduler ScheduleJob<T>(int intervalInSeconds, IEnumerable<object> objects = null) where T : IJob
        {
            IJobDetail job = Createjob<T>(objects);

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

        public IScheduler ScheduleJob<T>(string cronExpression, IEnumerable<object> objects = null) where T : IJob
        {
             IJobDetail job = Createjob<T>(objects);

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
            _quartzProps.Add("quartz.threadPool.threadCount", _jobs.Count.ToString());

            var factory = new StdSchedulerFactory(_quartzProps);

            var scheduler = await factory.GetScheduler();

            await scheduler.Start();

            foreach (var job in _jobs)
                await scheduler.ScheduleJob(job.Item1, job.Item2);
        }
        
        private static IJobDetail Createjob<T>(IEnumerable<object> objects) where T : IJob
        {
            JobDataMap instances = new JobDataMap();
            instances.Put("instances", objects);

            IJobDetail job = JobBuilder.Create<T>()
                               .UsingJobData(instances)
                               .WithIdentity(typeof(T).Name, "job-group")
                               .Build();
            return job;
        }
    }
}