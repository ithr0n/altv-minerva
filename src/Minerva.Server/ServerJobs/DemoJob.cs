using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Minerva.Server.ServerJobs.Base;

namespace Minerva.Server.ServerJobs
{
    public class DemoJob
        : IServerJob
    {
        private ILogger<DemoJob> Logger { get; }

        public DemoJob(ILogger<DemoJob> logger)
        {
            Logger = logger;
        }

        public async Task OnSave()
        {
            await Task.Run(() =>
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
