using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.DataAccessLayer.Models;
using PlayGermany.Server.DataAccessLayer.Services;
using PlayGermany.Server.Entities;

namespace PlayGermany.Server.Handlers
{
    public class SessionHandler
    {
        private readonly AccountService _accountService;
        private readonly CharacterService _characterService;

        private ILogger<SessionHandler> Logger { get; }
        public Vector3 SpawnPoint { get; }

        public SessionHandler(
            ILogger<SessionHandler> logger,
            IConfiguration configuration,
            AccountService accountService,
            CharacterService characterService)
        {
            _accountService = accountService;
            _characterService = characterService;

            AltAsync.OnPlayerConnect += (player, reason) => OnPlayerConnect(player as ServerPlayer, reason);
            Alt.OnPlayerDead += (player, killer, weapon) => OnPlayerDead(player as ServerPlayer, killer, weapon);
            Alt.OnClient<ServerPlayer, string>("Login:Authenticate", OnLoginAuthenticate);
            Alt.OnClient<ServerPlayer>("RequestSpawn", OnRequestSpawn);
            Alt.OnClient<ServerPlayer, Vector3>("RequestTeleport", OnRequestTeleport);

            Logger = logger;

            if (
                float.TryParse(configuration.GetSection("World:SpawnPoint:X").Value, out float spX) &&
                float.TryParse(configuration.GetSection("World:SpawnPoint:Y").Value, out float spY) &&
                float.TryParse(configuration.GetSection("World:SpawnPoint:Z").Value, out float spZ)
            )
            {
                SpawnPoint = new Vector3(spX, spY, spZ);
            }
        }

        private async Task OnPlayerConnect(ServerPlayer player, string reason)
        {
            var uiUrl = "http://resource/client/html/index.html";

#if DEBUG
            uiUrl = "http://localhost:8080/index.html";
#endif

            var socialClubId = player.SocialClubId;
            var ip = player.Ip;

            Logger.LogInformation("Connection: SID {socialClub} with IP {ip}", socialClubId, ip);
            Logger.LogDebug("Requesting UI from {url}", uiUrl);

            if (!await _accountService.Exists(socialClubId))
            {
                _ = AltAsync.Do(() => player.Kick("Du hast noch keinen Account auf diesem Server"));
                return;
            }

            player.EmitLocked("UiManager:Initialize", uiUrl);
        }

        private void OnPlayerDead(ServerPlayer player, IEntity killer, uint weapon)
        {
            OnRequestSpawn(player);
        }

        private async void OnLoginAuthenticate(ServerPlayer player, string password)
        {
            var playerCharacters = new Dictionary<int, string>();

            if (await _accountService.Authenticate(
                    player.SocialClubId,
                    player.HardwareIdHash,
                    player.HardwareIdExHash,
                    password,
                    out Account account))
            {
                player.Account = account;

                foreach (var character in _characterService.GetCharacters(account))
                {
                    playerCharacters.Add(character.Id, character.Name);
                }
            }

            player.Emit("Login:Callback", player.LoggedIn, playerCharacters);
        }

        private void OnRequestSpawn(ServerPlayer player)
        {
            player.Model = (uint) PedModel.FreemodeFemale01; // load from db
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
