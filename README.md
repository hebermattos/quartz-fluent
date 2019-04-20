how to (Program.cs)

``` 
var schedulerService = new SchedulerService();
                
await schedulerService
        .CreateInMemoryScheduler()
        .ScheduleJob<FooJob>(intervalInSeconds: 3)
        .ScheduleJob<BarJob>(cronExpression: "* * * * * ? *")
        .Run();
```