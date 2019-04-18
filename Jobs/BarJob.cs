using System;
using System.Threading.Tasks;
using Quartz;

public class BarJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
       return Console.Out.WriteLineAsync("BarJob executed!");
    }
}