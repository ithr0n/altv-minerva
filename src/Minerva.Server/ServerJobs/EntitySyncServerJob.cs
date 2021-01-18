using System.Threading.Tasks;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using Microsoft.Extensions.Logging;
using Minerva.Server.Core.Contracts.Abstractions;

namespace Minerva.Server.ServerJobs
{
    public class EntitySyncServerJob
        : IServerJob
    {
        private ILogger<EntitySyncServerJob> Logger { get; }

        public EntitySyncServerJob(ILogger<EntitySyncServerJob> logger)
        {
            Logger = logger;
        }

        public async Task OnSave()
        {
            await Task.CompletedTask;
        }

        public async Task OnShutdown()
        {
            AltEntitySync.Stop();

            await Task.CompletedTask;
        }

        public async Task OnStartup()
        {
            // initialize EntitySync
            // docs: http://csharp.altv.mp/articles/entity-sync.html

            AltEntitySync.Init(2, 100,
                (threadId) => false,
                (threadCount, repository) => new ServerEventNetworkLayer(threadCount, repository),
                (entity, threadCount) => (entity.Type % threadCount),
                (entityId, entityType, threadCount) => (entityType % threadCount),
                (threadId) => new LimitedGrid3(50_000, 50_000, 100, 10_000, 10_000, 300),
                new IdProvider());

            Logger.LogInformation("EntitySync initialized");

            await Task.CompletedTask;
        }
    }
}
