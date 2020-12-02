using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Entities;
using System;
using System.Linq;

namespace PlayGermany.Server.Handlers
{
    public class ClientConsoleHandler
    {
        private ILogger<ClientConsoleHandler> Logger { get; }

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
                    player.Emit("UiManager:Info", $"Aktuelle Position: {player.Position}");
                    player.Emit("UiManager:CopyToClipboard", player.Position.ToString());
                    break;

                case "tpwp":
                    player.Emit("ConsoleHandler:TeleportToWaypoint");
                    break;

                case "players":
                    Alt.GetAllPlayers().ToList().ForEach(otherPlayer =>
                    {
                        player.Emit("UiManager:Info", ((ServerPlayer)otherPlayer).RoleplayName);
                    });
                    break;

                case "tp":
                    if (args.Length < 1)
                    {
                        player.Emit("UiManager:Error", "Du musst ein Ziel angeben!");
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
                        player.Emit("UiManager:Error", "Spieler nicht gefunden oder nicht eindeutig");
                    }
                    break;

                case "broadcast":
                    if (args.Length < 1)
                    {
                        player.Emit("UiManager:Error", "Du musst eine Nachricht angeben!");
                        return;
                    }
                    var message = string.Join(' ', args);
                    Alt.EmitAllClients("UiManager:Notification", message);
                    break;

                case "car":
                    if (args.Length < 1)
                    {
                        player.Emit("UiManager:Error", "Du musst ein Fahrzeug angeben!");
                        return;
                    }

                    var pos = player.Position + new AltV.Net.Data.Position(3, 0, 0);
                    try
                    {
                        _ = new ServerVehicle(Alt.Hash(args[0]), pos, player.Rotation);
                    }
                    catch (Exception ex)
                    {
                        player.Emit("UiManager:Error", ex.Message);
                    }
                    break;

                case "speedcheat":
                    if (args.Length < 1 || !int.TryParse(args[0], out int engineMultiplier) || engineMultiplier < 0 || engineMultiplier > 100)
                    {
                        player.Emit("UiManager:Error", "Du musst einen Wert (0-100) angeben!");
                        return;
                    }
                    if (!player.IsInVehicle)
                    {
                        player.Emit("UiManager:Error", "Du musst in einem Fahrzeug sitzen!");
                        return;
                    }
                    player.Vehicle.SetStreamSyncedMetaData("EnginePowerMultiplier", engineMultiplier);
                    break;

                default:
                    player.Emit("UiManager:Error", "Dieser Befehl existiert nicht.");
                    break;
            }
        }
    }
}
