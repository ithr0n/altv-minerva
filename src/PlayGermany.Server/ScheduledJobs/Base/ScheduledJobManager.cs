using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PlayGermany.Server.ScheduledJobs.Base
{
    public class ScheduledJobManager
    {
        private readonly List<Action> _scheduledJobs;
        private readonly int _minimalIntervalMs = 500;
        private readonly ParallelOptions _parallelOptions;

        public CancellationTokenSource Cancellation { get; private set; }
        private ILogger<ScheduledJobManager> Logger { get; }

        public ScheduledJobManager(ILogger<ScheduledJobManager> logger, IEnumerable<ScheduledJob> scheduledJobs)
        {
            _scheduledJobs = new List<Action>();

            _parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 3,
                CancellationToken = Cancellation.Token
            };

            Logger = logger;

            foreach(var job in scheduledJobs)
            {
                _scheduledJobs.Add(() => {
                    if (job.LastExecution == DateTime.MinValue)
                    {
                        // skip first execution directly on server startup
                        job.LastExecution = DateTime.Now;
                        return;
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
                });
            }
        }

        public void EnableWorker()
        {
            Task.Run(() => DoWork());
        }

        private async Task DoWork()
        {
            while (!Cancellation.IsCancellationRequested)
            {
                Parallel.Invoke(_parallelOptions, _scheduledJobs.ToArray());

                if (!Cancellation.IsCancellationRequested)
                {
                    await Task.Delay(_minimalIntervalMs);
                }
            }
        }
    }
}
