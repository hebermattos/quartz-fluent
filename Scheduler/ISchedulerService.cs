using System.Threading.Tasks;
using Quartz;

namespace Scheduler
{
    public interface ISchedulerService 
    {             
         ISchedulerService ScheduleJob<T>(int intervalInSeconds) where T : IJob;

         ISchedulerService ScheduleJob<T>(string cronExpression) where T : IJob;

         Task Run();
    }
}