using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.ScheduledJobs.Base;
using PlayGermany.Server.ServerJobs.Base;
using System;
using System.Collections.Generic;

namespace PlayGermany.Server.ScheduledJobs
{
    public class WorldSaverScheduledJob
        : BaseScheduledJob
    {
        private readonly IEnumerable<IServerJob> _serverJobs;

        public ILogger<WorldSaverScheduledJob> Logger { get; }

        public WorldSaverScheduledJob(ILogger<WorldSaverScheduledJob> logger, IConfiguration serverConfig, IEnumerable<IServerJob> serverJobs)
            : base(TimeSpan.FromMilliseconds(double.Parse(serverConfig.GetSection("World:SaveInterval")?.Value ?? "300000")))
        {
            // if config not present then default of 5 minutes is applied in base constructor

            Logger = logger;
            _serverJobs = serverJobs;
        }

        public override void Action()
        {
            foreach (var job in _serverJobs)
            {
                job.OnSave();
            }

            Logger.LogTrace("World save at {CurrentDate}", DateTime.Now);
        }
    }
}
