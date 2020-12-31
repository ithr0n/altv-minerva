using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.ServerJobs.Base;

namespace PlayGermany.Server.ServerJobs
{
    public class DemoJob
        : IServerJob
    {
        private ILogger<DemoJob> Logger { get; }

        public DemoJob(ILogger<DemoJob> logger)
        {
            Logger = logger;
        }

        public Task OnSave()
        {
            return Task.Run(() =>
            {
                Logger.LogInformation("OnSave Demo");
            });
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
