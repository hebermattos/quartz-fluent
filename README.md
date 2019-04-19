how to

``` 
var logProvider = new ConsoleLog();
var schedulerService = new SchedulerService();

schedulerService
        .CreateInMemoryScheduler(
            instanceName: "my-scheduler", 
            threadCount: 2, 
            logProvider: logProvider
        ).ScheduleJob<FooJob>(intervalInSeconds: 2)
         .ScheduleJob<BarJob>(cronExpression: "* * * * * ? *");
```

output

```
[17:07:09] [Info] Using object serializer: Quartz.Simpl.BinaryObjectSerializer, Quartz
[17:07:09] [Info] Initialized Scheduler Signaller of type: Quartz.Core.SchedulerSignalerImpl
[17:07:09] [Info] Quartz Scheduler v.3.0.7.0 created.
[17:07:09] [Info] RAMJobStore initialized.
[17:07:09] [Info] Scheduler meta-data: Quartz Scheduler (v3.0.7.0) 'my-scheduler' with instanceId 'NON_CLUSTERED'
  Scheduler class: 'Quartz.Core.QuartzScheduler' - running locally.
  NOT STARTED.
  Currently in standby mode.
  Number of jobs executed: 0
  Using thread pool 'Quartz.Simpl.DefaultThreadPool' - with 2 threads.
  Using job-store 'Quartz.Simpl.RAMJobStore' - which does not support persistence. and is not clustered.

[17:07:09] [Info] Quartz scheduler 'my-scheduler' initialized
[17:07:09] [Info] Quartz scheduler version: 3.0.7.0
[17:07:09] [Info] Scheduler my-scheduler_$_NON_CLUSTERED started.
FooJob executed!
BarJob executed!
FooJob executed!
FooJob executed!
FooJob executed!
BarJob executed!
FooJob executed!
FooJob executed!
```