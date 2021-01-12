using System;

namespace Minerva.Server.PlayerJobs.Base
{
    public abstract class BaseJobStep
    {
        public BaseJobStep(PlayerJob job)
        {
            Job = job;
        }

        public PlayerJob Job { get; }

        public DateTime ActivatedAt { get; }
        public TimeSpan Deadline { get; }
        public DateTime FinishedAt { get; private set; }

        public string SuccessMessage { get; }
        public string FailedMessage { get; }

        public virtual bool Execute()
        {
            FinishedAt = DateTime.Now;

            return true;
        }
    }
}