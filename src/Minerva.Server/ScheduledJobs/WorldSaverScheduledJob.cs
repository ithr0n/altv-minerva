using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minerva.Server.Contracts.Configuration;
using Minerva.Server.ScheduledJobs.Base;
using Minerva.Server.ServerJobs.Base;

namespace Minerva.Server.ScheduledJobs
{
    public class WorldSaverScheduledJob
        : ScheduledJob
    {
        private readonly IEnumerable<IServerJob> _serverJobs;

        private ILogger<WorldSaverScheduledJob> Logger { get; }

        public WorldSaverScheduledJob(
            ILogger<WorldSaverScheduledJob> logger, 
            IOptions<GameOptions> gameOptions,
            IEnumerable<IServerJob> serverJobs)
            : base(TimeSpan.FromSeconds(gameOptions.Value.SaveInterval))
        {
            Logger = logger;
            _serverJobs = serverJobs;
        }

        public override async Task Action()
        {
            Logger.LogTrace($"World save initiated at {DateTime.Now}");

            // execute save method of all server jobs
            var taskList = new List<Task>();
            Parallel.ForEach(_serverJobs, job => taskList.Add(job.OnSave()));

            // wait until all jobs finished
            await Task.WhenAll(taskList.ToArray());

            Logger.LogTrace($"World save completed at {DateTime.Now}");
        }
    }
}
