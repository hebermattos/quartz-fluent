using System;
using System.Threading.Tasks;
using Quartz;

public class HelloJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
       return Console.Out.WriteLineAsync("Hello job");
    }
}