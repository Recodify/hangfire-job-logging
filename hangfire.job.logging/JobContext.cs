using System;
using Hangfire.Server;

namespace hangfire.job.logging
{
    public class JobContext : IServerFilter
    {
        [ThreadStatic]
        private static string _jobId;

        public static string JobId => _jobId;

        public void OnPerforming(PerformingContext filterContext)
        {
            _jobId = filterContext.BackgroundJob.Id;
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            _jobId = "";
        }
    }
}