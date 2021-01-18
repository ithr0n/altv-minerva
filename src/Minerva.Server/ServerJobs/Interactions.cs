using AltV.Net;
using AltV.Net.Interactions;
using Minerva.Server.Core.Contracts.Abstractions;
using Minerva.Server.Core.Entities;
using System.Threading.Tasks;

namespace Minerva.Server.ServerJobs
{
    public class Interactions
        : IServerJob
    {
        public Interactions()
        {
            Alt.OnClient<ServerPlayer>("Interactions:Trigger", OnInteractionsTriggered);
        }

        private void OnInteractionsTriggered(ServerPlayer player)
        {
            AltInteractions.TriggerInteractions(player);
        }

        public async Task OnSave()
        {
            await Task.CompletedTask;
        }

        public async Task OnShutdown()
        {
            AltInteractions.Dispose();

            await Task.CompletedTask;
        }

        public async Task OnStartup()
        {
            AltInteractions.Init();

            await Task.CompletedTask;
        }
    }
}
