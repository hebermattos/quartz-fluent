using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
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
                var logProvider = new ConsoleLog();
                var schedulerService = new SchedulerService();

                await schedulerService.Create("minha-instancia-agendador", 2, logProvider);
                await schedulerService.Start();

                await schedulerService.ScheduleJob<HelloJob>(2);
                await schedulerService.ScheduleJob<ByeJob>(7);

                Console.ReadLine();
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }
    }
}
