using System.Threading.Tasks;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.ServerJobs.Base;

namespace PlayGermany.Server.ServerJobs
{
    public class EntitySyncServerJob
        : IServerJob
    {
        private ILogger<EntitySyncServerJob> Logger { get; }

        public EntitySyncServerJob(ILogger<EntitySyncServerJob> logger)
        {
            Logger = logger;
        }

        public Task OnSave()
        {
            return Task.CompletedTask;
        }

        public void OnShutdown()
        {
            AltEntitySync.Stop();
        }

        public void OnStartup()
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
        }
    }
}
