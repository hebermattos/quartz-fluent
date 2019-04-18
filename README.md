``` 
var logProvider = new ConsoleLog();
var schedulerService = new SchedulerService();

await schedulerService.Create("my-scheduler", 2, logProvider);
await schedulerService.Start();

await schedulerService.ScheduleJob<FooJob>(2);
await schedulerService.ScheduleJob<BarJob>(7);
```