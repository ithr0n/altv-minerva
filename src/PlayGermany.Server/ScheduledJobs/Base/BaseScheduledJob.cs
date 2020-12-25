using System;
using System.Threading.Tasks;

namespace PlayGermany.Server.ScheduledJobs.Base
{
    public abstract class BaseScheduledJob
    {
        public string Id { get; }

        public TimeSpan Interval { get; }

        public DateTime LastExecution { get; set; }

        public BaseScheduledJob(TimeSpan interval)
        {
            Interval = interval;

            Id = Guid.NewGuid().ToString();
            LastExecution = DateTime.MinValue;
        }

        public abstract Task Action();
    }
}
