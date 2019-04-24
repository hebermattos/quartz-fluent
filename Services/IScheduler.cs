using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Services
{
    public interface IScheduler 
    {        
         IScheduler CreateInMemoryScheduler(ILogProvider logProvider);
         
         IScheduler CreateSqlServerScheduler(string connectionString, ILogProvider logProvider = null);

         IScheduler ScheduleJob<T>(int intervalInSeconds, IEnumerable<object> objects = null) where T : IJob;

         IScheduler ScheduleJob<T>(string cronExpression, IEnumerable<object> objects = null) where T : IJob;

         Task Run();
    }
}