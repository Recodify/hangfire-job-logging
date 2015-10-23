# Hangfire Job Logging

Simple implementation of a job logger that will allow you to log messages directly from your background job to the hangfire console. 

##### Status
Requires a small patch to hangfire, currently unmerged: https://github.com/HangfireIO/Hangfire/pull/466
This is however, a very tiny change which should allow you to make use of it before (if) it gets accepted.

## Usage

##### Invoking your job

Invoke your background job with:
```
BackgroundJob.Enqueue<IJob>(x => x.Execute(JobContext.Null));
```
##### Writing a message

In your background job, simply call `Write` on your instance of the logger.

```
...
public void Execute(IJobContext jobContext)
{
    jobLogger.Write(jobContext.Id, "hello from the job");
}
...
```

##### Getting an instance of IJobLogger

If you are making use of a DI container, simply inject register with your container and then inject `IJobLogger` into the your background job:

```
public class Job : IJob
{
    private readonly IJobLogger logger;
    public Job(IJobLogger logger)
    {
        this.logger = logger;
    }
}
```

If you are not using a DI Container, use either service location or direct instantiation in your background job:

```
public class Job : IJob
{
    private readonly IJobLogger logger;
    public Job(IJobLogger logger)
    {
        // Direct instantiation.
        this.logger = new JobLogger();
        // Service location;
        this.logger = ServiceLocator.Locate<IJobLogger>();
    }
}
```
