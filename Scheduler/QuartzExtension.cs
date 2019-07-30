using System;
using System.Collections.Specialized;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;

namespace Scheduler
{
    public static class QuartzExtensions
    {
        private static string _connectionString;
        private static int _threadCount;

        public static async void AddQuartz(this IServiceCollection services,
        SchedulerType type,
        int threadCount,
        string connectionString = null)
        {
            _connectionString = connectionString;
            _threadCount = threadCount;

            NameValueCollection props = GetProps(type);

            var factory = new StdSchedulerFactory(props);
            var scheduler = await factory.GetScheduler();

            services.AddSingleton<ISchedulerService, SchedulerService>();                   
            services.AddSingleton(scheduler);

            var jobFactory = new IoCJobFactory(services.BuildServiceProvider());

            scheduler.JobFactory = jobFactory;
        }

        private static NameValueCollection GetProps(SchedulerType type)
        {
            switch (type)
            {
                case SchedulerType.InMemory:
                    return new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" },
                    {"quartz.scheduler.instanceName" , Guid.NewGuid().ToString()} ,
                    {"quartz.jobStore.type" , "Quartz.Simpl.RAMJobStore, Quartz"},
                    {"quartz.threadPool.threadCount", _threadCount.ToString()}
                };
                case SchedulerType.SqlServer:
                    return new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" },
                    {"quartz.scheduler.instanceName" , Guid.NewGuid().ToString()} ,
                    {"quartz.jobStore.type" , "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz"},
                    {"quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.StdAdoDelegate"},
                    {"quartz.jobStore.tablePrefix", "QRTZ_"},
                    {"quartz.jobStore.dataSource", "default"},
                    {"quartz.dataSource.default.connectionString", _connectionString},
                    {"quartz.dataSource.default.provider", "SqlServer"},
                    {"quartz.jobStore.useProperties", "true"},
                    {"quartz.jobStore.lockHandler.type", "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz"},
                    {"quartz.threadPool.threadCount", _threadCount.ToString()}
                };

                default:
                    throw new ArgumentException(type.ToString());
            }
        }
    }
}