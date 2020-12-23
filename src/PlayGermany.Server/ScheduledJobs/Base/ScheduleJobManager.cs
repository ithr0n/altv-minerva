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
        private readonly int _minimalIntervalMs = 500;

        public CancellationTokenSource Cancellation { get; private set; }
        private ILogger<ScheduleJobManager> Logger { get; }

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
                Cancellation = new CancellationTokenSource();
                _worker.Start();
            }
        }

        private void OnWork()
        {
            while (!Cancellation.IsCancellationRequested)
            {
                foreach (var job in _scheduledJobs)
                {
                    if (Cancellation.IsCancellationRequested)
                    {
                        break;
                    }

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

                if (!Cancellation.IsCancellationRequested)
                {
                    Thread.Sleep(_minimalIntervalMs);
                }
            }
        }

        public BaseScheduledJob GetJob(string id)
        {
            return _scheduledJobs.FirstOrDefault(c => c.Id == id);
        }
    }
}
