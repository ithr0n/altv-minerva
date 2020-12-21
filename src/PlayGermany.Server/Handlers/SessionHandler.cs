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
            AltAsync.OnClient<ServerPlayer, int>("Session:RequestCharacterSpawn", OnRequestCharacterSpawn);
            Alt.OnClient<ServerPlayer, Vector3>("RequestTeleport", OnRequestTeleport);
            Alt.OnClient<ServerPlayer, Dictionary<string, string>>("Session:CreateNewCharacter", OnCreateNewCharacter);

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
                //_ = AltAsync.Do(() => player.Kick("Du hast noch keinen Account auf diesem Server"));
                player.Kick("Du hast noch keinen Account auf diesem Server"); // todo: test if that works in async context
                return;
            }

            player.EmitLocked("UiManager:Initialize", uiUrl);
        }

        private void OnPlayerDead(ServerPlayer player, IEntity killer, uint weapon)
        {
            OnRequestCharacterSpawn(player, player.Character.Id);
        }

        private async void OnLoginAuthenticate(ServerPlayer player, string password)
        {
            var playerCharacters = new List<Dictionary<string, string>>();

            var account = await _accountService.Authenticate(
                player.SocialClubId,
                player.HardwareIdHash,
                player.HardwareIdExHash,
                password);

            if (account != null)
            {
                player.Account = account;

                foreach (var character in _characterService.GetCharacters(account))
                {
                    var jsonObjSerialized = new Dictionary<string, string>();
                    jsonObjSerialized.Add("charId", character.Id.ToString());
                    jsonObjSerialized.Add("charName", character.Name);

                    playerCharacters.Add(jsonObjSerialized);
                }
            }

            player.Emit("Login:Callback", player.LoggedIn, playerCharacters);
        }

        private async void OnRequestCharacterSpawn(ServerPlayer player, int characterId)
        {
            var character = await _characterService.GetCharacter(characterId);

            if (character == null || character.AccountId != player.Account.SocialClubId)
            {
                player.Kick("Ungültiger Charakter");
                return;
            }

            player.Character = character;
            player.Model = Alt.Hash(character.Model); // load from db
            player.Dimension = 0;

            _ = player.SpawnAsync(SpawnPoint);

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

        private void OnCreateNewCharacter(ServerPlayer player, Dictionary<string, string> charCreationObj)
        {
            if (charCreationObj.TryGetValue("firstName", out string firstName) &&
                charCreationObj.TryGetValue("lastName", out string lastName) &&
                charCreationObj.TryGetValue("model", out string model) &&
                charCreationObj.TryGetValue("appearanceParents", out string appearanceParents) &&
                charCreationObj.TryGetValue("appearanceFaceFeatures", out string appearanceFaceFeatures) &&
                charCreationObj.TryGetValue("appearanceDetails", out string appearanceDetails) &&
                charCreationObj.TryGetValue("appearanceHair", out string appearanceHair) &&
                charCreationObj.TryGetValue("appearanceClothes", out string appearanceClothes))
            {
                var character = new Character();
                character.AccountId = player.Account.SocialClubId;
                character.FirstName = firstName;
                character.LastName = lastName;
                character.Model = model;
                character.Armor = 0;
                character.Health = 200;
                character.Cash = 500;
                character.Hunger = 100;
                character.Thirst = 100;
                character.AppearanceParents = appearanceParents;
                character.AppearanceFaceFeatures = appearanceFaceFeatures;
                character.AppearanceDetails = appearanceDetails;
                character.AppearanceHair = appearanceHair;
                character.AppearanceClothes = appearanceClothes;

                _characterService.Create(character);
            }
        }
    }
}
