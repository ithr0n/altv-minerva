using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using PlayGermany.Server.Entities;

namespace PlayGermany.Server
{
    public class Server
        : AsyncResource
    {
        private readonly Kernel _kernel;

        public Server()
        {
            _kernel = new Kernel();
            _kernel.Initialize();
        }

        public override void OnStart()
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

            _kernel.Startup();
        }

        public override void OnTick()
        {
            base.OnTick();
        }

        public override void OnStop()
        {
            _kernel.Shutdown();
        }

        #region Entities

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new ServerPlayerFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new ServerVehicleFactory();
        }

        #endregion
    }
}
