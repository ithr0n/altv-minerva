using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Callbacks;
using PlayGermany.Server.Entities;
using PlayGermany.Server.EntitySync.Streamers;
using PlayGermany.Server.Enums;
using PlayGermany.Server.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            AltAsync.OnClient<ServerPlayer, string, string[]>("ClientConsoleHandler:Command", OnCommand);
        }

        private async void OnCommand(ServerPlayer player, string command, string[] args = null)
        {
            args ??= Array.Empty<string>();

            Logger.LogInformation($"OnCommand by {player.RoleplayName}: {command} with args: {string.Join(';', args)}");

            switch (command.ToLower())
            {
                case "pos":
                    {
                        player.Notify($"Aktuelle Position: {player.Position}");
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
                        var lambda = new AsyncFunctionCallback<IPlayer>(async (IPlayer otherPlayer) =>
                        {
                            if (otherPlayer is ServerPlayer serverPlayer)
                            {
                                player.Notify(serverPlayer.RoleplayName);
                            }
                        });

                        await Alt.ForEachPlayers(lambda);

                        break;
                    }

                case "tp":
                    {
                        if (args.Length < 1)
                        {
                            player.Notify("Du musst ein Ziel angeben!", NotificationType.Error);
                            return;
                        }

                        var target = Alt.GetAllPlayers().SingleOrDefault(player => ((ServerPlayer)player).RoleplayName.Contains(args[0]));

                        if (target != null && target != this)
                        {
                            lock (target)
                            {
                                if (target != null && target.Exists)
                                {
                                    player.Position = target.Position + new Position(1, 1, 0.5f);

                                    if (target.IsInVehicle)
                                    {
                                        player.Emit("VehicleHandler:TeleportInto", target.Vehicle);
                                    }
                                }
                                else
                                {
                                    player.Notify("Da ging etwas schief...", NotificationType.Warning);
                                }
                            }
                        }
                        else
                        {
                            player.Notify("Spieler nicht gefunden oder nicht eindeutig", NotificationType.Error);
                        }

                        break;
                    }

                case "broadcast":
                    {
                        if (args.Length < 1)
                        {
                            player.Notify("Du musst eine Nachricht angeben!", NotificationType.Error);
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
                            player.Notify("Du musst ein Fahrzeug angeben!, NotificationType.Error");
                            return;
                        }

                        if (!uint.TryParse(args[0], out uint hash))
                        {
                            hash = Alt.Hash(args[0]);
                        }

                        var pos = player.Position + new AltV.Net.Data.Position(3, 0, 0);
                        try
                        {
                            await AltAsync.Do(() =>
                            {
                                _ = new ServerVehicle(hash, pos, player.Rotation);
                            });
                        }
                        catch (Exception ex)
                        {
                            player.Notify(ex.Message, NotificationType.Error);
                        }

                        break;
                    }

                case "speedcheat":
                    {
                        if (args.Length < 1 || !int.TryParse(args[0], out int engineMultiplier) || engineMultiplier < 0 || engineMultiplier > 100)
                        {
                            player.Notify("Du musst einen Wert (0-100) angeben!", NotificationType.Error);
                            return;
                        }

                        if (!player.IsInVehicle)
                        {
                            player.Notify("Du musst in einem Fahrzeug sitzen!", NotificationType.Error);
                            return;
                        }

                        await player.Vehicle.SetStreamSyncedMetaDataAsync("EnginePowerMultiplier", engineMultiplier);

                        break;
                    }

                case "prop":
                    {
                        if (args.Length < 1)
                        {
                            player.Notify("Du musst ein Objekt angeben!", NotificationType.Error);
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
                            player.Notify(ex.Message, NotificationType.Error);
                        }

                        break;
                    }

                case "weather":
                    {
                        if (args.Length < 1 || !uint.TryParse(args[0], out uint weatherId))
                        {
                            player.Notify("Du musst eine Wetter ID und optional die Tageszeit (Stunden) angeben!", NotificationType.Error);
                            return;
                        }

                        if (!Enum.IsDefined(typeof(WeatherType), weatherId))
                        {
                            player.Notify("Ungültiges Wetter angegeben!", NotificationType.Error);
                            return;
                        }

                        bool immediately = false;

                        if (args.Length >= 2 && !bool.TryParse(args[1], out immediately))
                        {
                            player.Notify("Ungültiger Parameter (immediately)", NotificationType.Error);
                            return;
                        }

                        _worldData.Weather = (WeatherType)weatherId;

                        if (immediately)
                        {
                            await Task.Delay(500);

                            var lambda = new AsyncFunctionCallback<IPlayer>(async (IPlayer p) =>
                            {
                                p.Emit("World:SetWeatherImmediately");
                                await Task.CompletedTask;
                            });

                            await Alt.ForEachPlayers(lambda);
                        }

                        break;
                    }

                case "clock":
                    {
                        if (args.Length < 1 || !int.TryParse(args[0], out int hours) || hours < 0 && hours > 23)
                        {
                            player.Notify("Du musst eine Stundenanzahl (0-23) angeben!", NotificationType.Error);
                            return;
                        }

                        _worldData.Clock = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, 0, 0);

                        if (args.Length >= 2 && bool.TryParse(args[1], out bool clockEnabled))
                        {
                            _worldData.ClockPaused = clockEnabled;
                        }

                        var lambda = new AsyncFunctionCallback<IPlayer>(async (IPlayer p) =>
                        {
                            await p.SetDateTimeAsync(_worldData.Clock);
                        });

                        await Alt.ForEachPlayers(lambda);

                        break;
                    }

                case "blackout":
                    {
                        lock (_worldData)
                        {
                            _worldData.Blackout = !_worldData.Blackout;
                        }

                        break;
                    }

                default:
                    player.Notify("Dieser Befehl existiert nicht.", NotificationType.Error);
                    break;
            }
        }
    }
}
