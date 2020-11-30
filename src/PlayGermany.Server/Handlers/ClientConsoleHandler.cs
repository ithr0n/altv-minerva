using AltV.Net;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Entities;
using System;
using System.Linq;

namespace PlayGermany.Server.Handlers
{
    public class ClientConsoleHandler
    {
        public ILogger<ClientConsoleHandler> Logger { get; }

        public ClientConsoleHandler(ILogger<ClientConsoleHandler> logger)
        {
            Logger = logger;

            Alt.OnClient<ServerPlayer, string, string[]>("ClientConsoleHandler:Command", OnCommand);
        }

        private void OnCommand(ServerPlayer player, string command, string[] args = null)
        {
            args ??= Array.Empty<string>();

            Logger.LogInformation($"OnCommand by {player.RoleplayName}: {command} with args: {string.Join(';', args)}");
            
            switch (command.ToLower())
            {
                case "pos":
                    player.Emit("UiManager:Notification", $"Aktuelle Position: {player.Position}");
                    break;

                case "tpwp":
                    player.Emit("ConsoleHandler:TeleportToWaypoint");
                    break;

                case "players":
                    Alt.GetAllPlayers().ToList().ForEach(otherPlayer =>
                    {
                        player.Emit("UiManager:Notification", ((ServerPlayer)otherPlayer).RoleplayName);
                    });
                    break;

                case "tp":
                    if (args.Length < 1)
                    {
                        player.Emit("UiManager:Notification", "Du musst ein Ziel angeben");
                        return;
                    }

                    var target = Alt.GetAllPlayers().SingleOrDefault(player => ((ServerPlayer)player).RoleplayName.Contains(args[0]));

                    if (target != null)
                    {
                        if (target == this)
                        {
                            return;
                        }

                        player.Position = target.Position + new AltV.Net.Data.Position(1, 1, 0.5f);

                        if (target.IsInVehicle)
                        {
                            player.Emit("VehicleHandler:TeleportInto", target.Vehicle);
                        }
                    }
                    else
                    {
                        player.Emit("UiManager:Notification", "Spieler nicht gefunden oder nicht eindeutig");
                    }
                    break;

                default:
                    player.Emit("UiManager:Notification", "Dieser Befehl existiert nicht.");
                    break;
            }
        }
    }
}
