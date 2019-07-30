how to (Program.cs)

``` 
var services = new ServiceCollection();

services.AddTransient<FooJob>();
services.AddTransient<BarJob>()

services.AddQuartz(SchedulerType.InMemory, 2)

var serviceProvider = services.BuildServiceProvider()

var schedulerService = serviceProvider.GetService<ISchedulerService>()

await schedulerService
        .ScheduleJob<FooJob>(intervalInSeconds: 3)
        .ScheduleJob<BarJob>(cronExpression: "* * * * * ? *")
        .Run();
```