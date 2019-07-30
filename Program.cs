using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Servico;
using util;

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
            var services = new ServiceCollection();

            services.AddTransient<FooJob>();
            services.AddTransient<BarJob>();

            services.AddQuartz(SchedulerType.InMemory, 2);

            var serviceProvider = services.BuildServiceProvider();

            var schedulerService = serviceProvider.GetService<ISchedulerService>();

            await schedulerService
                    .ScheduleJob<FooJob>(intervalInSeconds: 3)
                    .ScheduleJob<BarJob>(cronExpression: "* * * * * ? *")
                    .Run();

            Console.ReadLine();
        }
    }
}

