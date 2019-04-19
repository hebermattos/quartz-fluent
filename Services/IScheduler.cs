using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Services
{
    public interface IScheduler 
    {        
         IScheduler CreateInMemoryScheduler(string instanceName, int threadCount, ILogProvider logProvider);

         IScheduler ScheduleJob<T>(int intervalInSeconds) where T : IJob;

         IScheduler ScheduleJob<T>(string cronExpression) where T : IJob;
    }
}