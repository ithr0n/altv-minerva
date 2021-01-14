using System;
using System.Threading.Tasks;

namespace Minerva.Server.Core.ScheduledJob
{
    public abstract class ScheduledJob
    {
        public string Id { get; }

        public TimeSpan Interval { get; }

        public DateTime LastExecution { get; set; }

        public ScheduledJob(TimeSpan interval)
        {
            Interval = interval;

            Id = Guid.NewGuid().ToString();
            LastExecution = DateTime.MinValue;
        }

        public abstract Task Action();
    }
}
