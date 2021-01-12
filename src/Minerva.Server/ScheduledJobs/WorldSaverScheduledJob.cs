using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
            GameOptions gameOptions,
            IEnumerable<IServerJob> serverJobs)
            : base(TimeSpan.FromSeconds(gameOptions.SaveInterval))
        {
            Logger = logger;
            _serverJobs = serverJobs;
        }

        public override async Task Action()
        {
            Logger.LogTrace("World save initiated at {CurrentDate}", DateTime.Now);

            var taskList = new List<Task>();

            foreach (var job in _serverJobs)
            {
                taskList.Add(job.OnSave());
            }

            await Task.WhenAll(taskList.ToArray());

            Logger.LogTrace("World save completed at {CurrentDate}", DateTime.Now);
        }
    }
}
