namespace hangfire.job.logging
{
    public interface IJobLogger
    {
       void Write(string jobId, string message);
    }
}