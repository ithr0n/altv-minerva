using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Minerva.Server.Core.Contracts.Abstractions;

namespace Minerva.Server.ServerJobs
{
    public class Demo
        : IServerJob
    {
        private ILogger<Demo> Logger { get; }

        public Demo(ILogger<Demo> logger)
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
