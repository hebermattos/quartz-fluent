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
                var schedulerService = new SchedulerService();
                
                await schedulerService
                        .CreateInMemoryScheduler()
                        //.CreateSqlServerScheduler("Server=localhost\\SQLEXPRESS;Database=quartz;Trusted_Connection=True;")
                        .ScheduleJob<FooJob>(intervalInSeconds: 3)
                        .ScheduleJob<BarJob>(cronExpression: "* * * * * ? *")
                        .Run();

                Console.ReadLine();
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }
    }
}

