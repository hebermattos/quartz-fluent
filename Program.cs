using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Services;

namespace quartz_hello_world
{
    class Program
    {
        static void Main(string[] args)
        {
            RunProgram().GetAwaiter().GetResult();
        }

        private static async Task RunProgram()
        {
            try
            {
                var schedulerService = new SchedulerService();
                var scheduler = await schedulerService.Create();

                await scheduler.Start();

                var jobService = new JobService();

                var jobList = new List<(IJobDetail, ITrigger)>(){
                    jobService.GetJobConfig<HelloJob>(2),
                    jobService.GetJobConfig<ByeJob>(7)
                };

                foreach (var job in jobList)
                    await scheduler.ScheduleJob(job.Item1, job.Item2);

                Console.ReadLine();
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }
    }
}
