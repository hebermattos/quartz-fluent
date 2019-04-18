using System;
using System.Threading.Tasks;
using Quartz;

public class FooJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
       return Console.Out.WriteLineAsync("FooJob executed!");
    }
}