using Microsoft.Extensions.Logging;
using PlayGermany.Server.ServerJobs.Base;

namespace PlayGermany.Server.ServerJobs
{
    public class DemoJob
        : IServerJob
    {
        public ILogger<DemoJob> Logger { get; }

        public DemoJob(ILogger<DemoJob> logger)
        {
            Logger = logger;
        }

        public void OnSave()
        {
            Logger.LogInformation("OnSave Demo");
        }

        public void OnShutdown()
        {
            Logger.LogInformation("OnShutdown Demo");
        }

        public void OnStartup()
        {
            Logger.LogInformation("OnStartup Demo");
        }
    }
}
