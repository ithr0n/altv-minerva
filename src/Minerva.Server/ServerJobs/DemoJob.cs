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

        public async Task OnShutdown()
        {
            Logger.LogInformation("OnShutdown Demo");

            await Task.CompletedTask;
        }

        public async Task OnStartup()
        {
            Logger.LogInformation("OnStartup Demo");

            await Task.CompletedTask;
        }
    }
}
