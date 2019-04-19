using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Services
{
    public interface IScheduler 
    {        
         Task CreateInMemoryScheduler(string instanceName, int threadCount, ILogProvider logProvider);

         IScheduler Start();

         IScheduler ScheduleJob<T>(int intervalInSeconds) where T : IJob;

         IScheduler ScheduleJob<T>(string cronExpression) where T : IJob;
    }
}