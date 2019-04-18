using System;
using System.Threading.Tasks;
using Quartz;

public class ByeJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
       return Console.Out.WriteLineAsync("Bye job");
    }
}