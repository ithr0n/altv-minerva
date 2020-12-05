using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Entities;
using PlayGermany.Server.EntitySync.Streamers;
using PlayGermany.Server.ScheduledJobs;
using System;
using System.Linq;
using System.Numerics;

namespace PlayGermany.Server.Handlers
{
    public class ClientConsoleHandler
    {
        private readonly WorldData _worldData;

        private ILogger<ClientConsoleHandler> Logger { get; }
        public PropsStreamer PropsStreamer { get; }

        public ClientConsoleHandler(
            ILogger<ClientConsoleHandler> logger,
            PropsStreamer propsStreamer,
            WorldData worldData)
        {
            Logger = logger;
            PropsStreamer = propsStreamer;
            _worldData = worldData;
            Alt.OnClient<ServerPlayer, string, string[]>("ClientConsoleHandler:Command", OnCommand);
        }

        private void OnCommand(ServerPlayer player, string command, string[] args = null)
        {
            args ??= Array.Empty<string>();

            Logger.LogInformation($"OnCommand by {player.RoleplayName}: {command} with args: {string.Join(';', args)}");

            switch (command.ToLower())
            {
                case "pos":
                    {
                        player.Emit("UiManager:Info", $"Aktuelle Position: {player.Position}");
                        player.Emit("UiManager:CopyToClipboard", player.Position.ToString());

                        break;
                    }

                case "tpwp":
                    {
                        player.Emit("ConsoleHandler:TeleportToWaypoint");

                        break;
                    }

                case "players":
                    {
                        Alt.GetAllPlayers().ToList().ForEach(otherPlayer =>
                        {
                            player.Emit("UiManager:Info", ((ServerPlayer)otherPlayer).RoleplayName);
                        });

                        break;
                    }

                case "tp":
                    {
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
                    }

                case "broadcast":
                    {
                        if (args.Length < 1)
                        {
                            player.Emit("UiManager:Error", "Du musst eine Nachricht angeben!");
                            return;
                        }

                        var message = string.Join(' ', args);
                        Alt.EmitAllClients("UiManager:Notification", message);

                        break;
                    }

                case "car":
                    {
                        if (args.Length < 1)
                        {
                            player.Emit("UiManager:Error", "Du musst ein Fahrzeug angeben!");
                            return;
                        }

                        if (!uint.TryParse(args[0], out uint hash))
                        {
                            hash = Alt.Hash(args[0]);
                        }

                        var pos = player.Position + new AltV.Net.Data.Position(3, 0, 0);
                        try
                        {
                            _ = new ServerVehicle(hash, pos, player.Rotation);
                        }
                        catch (Exception ex)
                        {
                            player.Emit("UiManager:Error", ex.Message);
                        }

                        break;
                    }

                case "speedcheat":
                    {
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
                    }

                case "prop":
                    {
                        if (args.Length < 1)
                        {
                            player.Emit("UiManager:Error", "Du musst ein Objekt angeben!");
                            return;
                        }

                        if (!uint.TryParse(args[0], out uint hash))
                        {
                            hash = Alt.Hash(args[0]);
                        }

                        try
                        {
                            PropsStreamer.Create(hash, player.Position - new Position(0, 0, 1), player.Rotation, freezed: true);
                        }
                        catch (Exception ex)
                        {
                            player.Emit("UiManager:Error", ex.Message);
                        }

                        break;
                    }

                case "weather":
                    {
                        if (args.Length < 1 || !uint.TryParse(args[0], out uint weatherId))
                        {
                            player.Emit("UiManager:Error", "Du musst eine Wetter ID und optional die Tageszeit (Stunden) angeben!");
                            return;
                        }

                        if (!Enum.IsDefined(typeof(WeatherType), weatherId))
                        {
                            player.Emit("UiManager:Error", "Ungültiges Wetter angegeben!");
                            return;
                        }

                        _worldData.Weather = (WeatherType)weatherId;

                        if (args.Length >= 2)
                        {
                            if (!int.TryParse(args[1], out int hours))
                            {
                                player.Emit("UiManager:Error", "Zeit wurde nicht gesetzt, ungültiges Format angegeben!");
                                return;
                            }

                            _worldData.Clock = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, 0, 0);

                            if (args.Length >= 3 && bool.TryParse(args[2], out bool clockEnabled))
                            {
                                _worldData.ClockPaused = clockEnabled;
                            }
                        }

                        break;
                    }

                default:
                    player.Emit("UiManager:Error", "Dieser Befehl existiert nicht.");
                    break;
            }
        }
    }
}
