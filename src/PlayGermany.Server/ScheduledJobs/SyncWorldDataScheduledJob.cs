using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using PlayGermany.Server.Callbacks;
using PlayGermany.Server.Entities;
using PlayGermany.Server.ScheduledJobs.Base;
using System;
using System.Threading.Tasks;

namespace PlayGermany.Server.ScheduledJobs
{
    public class SyncWorldDataScheduledJob
        : ScheduledJob
    {
        private readonly WorldData _worldData;

        private DateTime _lastTick;

        public SyncWorldDataScheduledJob(WorldData worldData)
            : base(TimeSpan.FromSeconds(3d))
        {
            _worldData = worldData;

            Alt.OnPlayerConnect += (player, reason) => OnPlayerConnect(player as ServerPlayer, reason);
        }

        public override async Task Action()
        {
            if (_lastTick == DateTime.MinValue)
            {
                // skip first tick after server start
                _lastTick = DateTime.Now;
                return;
            }

            if (!_worldData.ClockPaused)
            {
                _worldData.Clock += DateTime.Now - _lastTick;
            }

            var callback = new AsyncFunctionCallback<IPlayer>(async (player) =>
            {
                player.SetDateTime(
                    _worldData.Clock.Day,
                    _worldData.Clock.Month,
                    _worldData.Clock.Year,
                    _worldData.Clock.Hour,
                    _worldData.Clock.Minute,
                    _worldData.Clock.Second);

                //player.SetWeather((uint)_worldData.Weather);

                // if weather transitions not working correctly, check _lastTickWeather != CurrentWeather
                // and then emit event to all clients, which calls in a loop (0 to 100):
                // natives.setWeatherTypeTransition(alt.hash(oldWeather), alt.hash(currentWeather), loopStep);


                // additional cool stuff (client side):
                /*
                if(weather === 'XMAS') {
                  native.setForceVehicleTrails(true);
                  native.setForcePedFootstepsTracks(true);
                } else {
                  native.setForceVehicleTrails(false);
                  native.setForcePedFootstepsTracks(false);
                }
                */

                await Task.CompletedTask;
            });

            await Alt.ForEachPlayers(callback);

            _lastTick = DateTime.Now;
        }

        private void OnPlayerConnect(ServerPlayer player, string reason)
        {
            player.SetDateTime(
                    _worldData.Clock.Day,
                    _worldData.Clock.Month,
                    _worldData.Clock.Year,
                    _worldData.Clock.Hour,
                    _worldData.Clock.Minute,
                    _worldData.Clock.Second);

            player.SetWeather((uint)_worldData.Weather);
        }
    }
}
