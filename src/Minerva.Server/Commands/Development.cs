using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Minerva.Server.Core.CommandSystem;
using Minerva.Server.EntitySync.Streamers;
using Minerva.Server.Extensions;
using System;
using Minerva.Server.Core.Entities;
using Minerva.Server.Core.Contracts.Enums;
using Minerva.Server.Core.Contracts.Abstractions.ScriptStrategy;
using Minerva.Server.Core.Callbacks;

namespace Minerva.Server.Commands
{
    public class Development
        : ISingletonScript
    {
        private readonly PropsStreamer _propsStreamer;
        private readonly WorldData _worldData;

        private bool _traceLogRunning;

        public Development(
            PropsStreamer propsStreamer,
            WorldData worldData)
        {
            _propsStreamer = propsStreamer;
            _worldData = worldData;
        }

        [Command("StartTrace", AccessLevel.Developer)]
        public void OnStartTraceCommand(ServerPlayer player, string traceName = "TraceLog")
        {
            if (_traceLogRunning)
            {
                player.Notify("Trace Log already running!", NotificationType.Error);
                return;
            }

            AltTrace.Start(traceName);
            _traceLogRunning = true;

            player.Notify($"Trace Logging started ({traceName}.nettrace)", NotificationType.Info);
        }

        [Command("StopTrace", AccessLevel.Developer)]
        public void OnStopTraceCommand(ServerPlayer player)
        {
            if (!_traceLogRunning)
            {
                player.Notify("No active Trace Log running...", NotificationType.Warning);
                return;
            }

            AltTrace.Stop();
            _traceLogRunning = false;

            player.Notify("Trace Logging stopped.", NotificationType.Info);
        }

        [Command("hash", AccessLevel.Developer, true)]
        public void OnHashCommand(ServerPlayer player, string text)
        {
            player.Notify(Alt.Hash(text).ToString(), NotificationType.Info);
        }

        [Command("pos", AccessLevel.Developer)]
        public void OnPositionCommand(ServerPlayer player)
        {
            player.Notify($"Aktuelle Position: {player.Position}");
            player.Emit("UiManager:CopyToClipboard", player.Position.ToString());
        }

        [Command("car", AccessLevel.Developer)]
        public void OnCarCommand(ServerPlayer player, string model = null)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                player.Notify("Du musst ein Fahrzeug angeben!, NotificationType.Error");
                return;
            }

            if (!uint.TryParse(model, out uint hash))
            {
                hash = Alt.Hash(model);
            }

            var pos = player.Position + new Position(3, 0, 0);

            try
            {
                _ = new ServerVehicle(hash, pos, player.Rotation);
            }
            catch (Exception ex)
            {
                player.Notify(ex.Message, NotificationType.Error);
            }
        }

        [Command("speedhack", AccessLevel.Developer)]
        public void OnSpeedHackCommand(ServerPlayer player, int engineMultiplier = -1)
        {
            if (!player.IsInVehicle)
            {
                player.Notify("Du musst in einem Fahrzeug sitzen!", NotificationType.Error);
                return;
            }

            if (engineMultiplier < 0 || engineMultiplier > 100)
            {
                player.Notify("Du musst einen Wert (0-100) angeben!", NotificationType.Error);
                return;
            }

            player.Vehicle.SetStreamSyncedMetaData("EnginePowerMultiplier", engineMultiplier);
        }

        [Command("prop", AccessLevel.Developer, true)]
        public void OnCreateSyncedPropCommand(ServerPlayer player, string model = null)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                player.Notify("Du musst ein Objekt angeben!", NotificationType.Error);
                return;
            }

            if (!uint.TryParse(model, out uint hash))
            {
                hash = Alt.Hash(model);
            }

            try
            {
                _propsStreamer.Create(hash, player.Position - new Position(0, 0, 1), player.Rotation, freezed: true);
            }
            catch (Exception ex)
            {
                player.Notify(ex.Message, NotificationType.Error);
            }
        }

        [Command("weather", AccessLevel.Developer)]
        public void OnChangeWeatherCommand(ServerPlayer player, int weatherId = -1, bool immediately = true)
        {
            if (!Enum.IsDefined(typeof(WeatherType), weatherId))
            {
                player.Notify("Ungültige WetterID angegeben (mögliche Werte: 0-14)!", NotificationType.Error);
                return;
            }

            _worldData.Weather = (WeatherType)weatherId;

            if (immediately)
            {
                /*await Task.Delay(500);

                var lambda = new AsyncFunctionCallback<IPlayer>(async (IPlayer p) =>
                {
                    p.Emit("World:SetWeatherImmediately");
                    await Task.CompletedTask;
                });

                await Alt.ForEachPlayers(lambda);*/
            }
        }

        [Command("time", AccessLevel.Developer)]
        public void OnChangeTimeCommand(ServerPlayer player, int hours = -1, bool freezeClock = false)
        {
            if (hours < 0 || hours > 23)
            {
                player.Notify("Du musst eine gültige Stundenanzahl (0-23) angeben!", NotificationType.Error);
                return;
            }

            _worldData.Clock = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, 0, 0);

            _worldData.ClockPaused = freezeClock;

            /*var lambda = new AsyncFunctionCallback<IPlayer>(async (IPlayer p) =>
            {
                await p.SetDateTimeAsync(_worldData.Clock);
            });

            await Alt.ForEachPlayers(lambda);*/

            var lambda = new FunctionCallback<IPlayer>((p) =>
            {
                p.SetDateTime(_worldData.Clock);
            });

            Alt.ForEachPlayers(lambda);
        }

        [Command("blackout", AccessLevel.Developer)]
        public void OnBlackoutCommand(ServerPlayer player)
        {
            _worldData.Blackout = !_worldData.Blackout;
        }
    }
}
