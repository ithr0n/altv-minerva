using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PlayGermany.Server.ScheduledJobs.Base
{
    public class ScheduleJobManager
    {
        private readonly ConcurrentBag<BaseScheduledJob> _scheduledJobs;
        private readonly Thread _worker;
        private readonly int _minimalInterval = 1;

        public ILogger<ScheduleJobManager> Logger { get; }

        public ScheduleJobManager(ILogger<ScheduleJobManager> logger, IEnumerable<BaseScheduledJob> scheduledJobs)
        {
            _scheduledJobs = new ConcurrentBag<BaseScheduledJob>(scheduledJobs);

            _worker = new Thread(OnWork) { IsBackground = true };
            Logger = logger;
        }

        public void EnableWorker()
        {
            if (_worker != null && !_worker.IsAlive)
            {
                _worker.Start();
            }
        }

        private void OnWork()
        {
            while (true)
            {
                foreach (var job in _scheduledJobs)
                {
                    if (job.LastExecution == DateTime.MinValue)
                    {
                        // skip first execution directly on server startup
                        job.LastExecution = DateTime.Now;
                    }

                    try
                    {
                        if (job.LastExecution + job.Interval < DateTime.Now)
                        {
                            job.Action();
                            job.LastExecution = DateTime.Now;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Exception on scheduled job {job.GetType().Name}: {ex.Message}\n{ex.StackTrace}");

                        if (ex.InnerException != null)
                        {
                            Logger.LogError($"{ex.InnerException.Message}\n{ex.InnerException.StackTrace}");
                        }
                    }
                }

                Thread.Sleep(_minimalInterval * 1000);
            }
        }

        public BaseScheduledJob GetJob(string id)
        {
            return _scheduledJobs.FirstOrDefault(c => c.Id == id);
        }
    }
}
