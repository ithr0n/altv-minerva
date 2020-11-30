using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Entities;
using System.Numerics;

namespace PlayGermany.Server.Handlers
{
    public class SessionHandler
    {
        public ILogger<SessionHandler> Logger { get; }
        public IConfiguration Configuration { get; }
        public Vector3 SpawnPoint { get; }

        public SessionHandler(ILogger<SessionHandler> logger, IConfiguration configuration)
        {
            Alt.OnPlayerConnect += (player, reason) => OnPlayerConnect(player as ServerPlayer, reason);
            Alt.OnPlayerDead += (player, killer, weapon) => OnPlayerDead(player as ServerPlayer, killer, weapon);
            Alt.OnClient<ServerPlayer>("RequestSpawn", OnRequestSpawn);
            Alt.OnClient<ServerPlayer, Vector3>("RequestTeleport", OnRequestTeleport);

            Logger = logger;
            Configuration = configuration;

            if (
                float.TryParse(Configuration.GetSection("World:SpawnPoint:X").Value, out float spX) &&
                float.TryParse(Configuration.GetSection("World:SpawnPoint:Y").Value, out float spY) &&
                float.TryParse(Configuration.GetSection("World:SpawnPoint:Z").Value, out float spZ)
                )
            {
                SpawnPoint = new Vector3(spX, spY, spZ);
            }
        }

        private void OnPlayerConnect(ServerPlayer player, string reason)
        {
            var uiUrl = "http://resource/client/html/index.html";

#if DEBUG
            uiUrl = "http://localhost:8080/index.html";
#endif

            Logger.LogInformation("Player connect: {socialClub} ({socialClubId})", player.Name, player.SocialClubId);
            Logger.LogDebug("Requesting UI from {url}", uiUrl);

            player.Emit("UiManager:Initialize", uiUrl);
        }

        private void OnPlayerDead(ServerPlayer player, IEntity killer, uint weapon)
        {
            OnRequestSpawn(player);
        }

        private void OnRequestSpawn(ServerPlayer player)
        {
            player.Model = (uint)PedModel.FreemodeFemale01; // load from db
            player.Dimension = 0;
            player.SpawnAsync(SpawnPoint);

            player.RoleplayName = $"Spieler {Alt.GetAllPlayers().Count}";

            player.Emit("PlayerSpawned");
        }

        private void OnRequestTeleport(ServerPlayer player, Vector3 targetPosition)
        {
            if (player.IsInVehicle)
            {
                player.Vehicle.Position = targetPosition;
            }
            else
            {
                player.Position = targetPosition;
            }
        }
    }
}
