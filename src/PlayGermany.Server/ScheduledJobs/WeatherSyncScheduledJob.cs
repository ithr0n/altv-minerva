using AltV.Net;
using AltV.Net.Enums;
using PlayGermany.Server.Entities;
using PlayGermany.Server.ScheduledJobs.Base;
using System;

namespace PlayGermany.Server.ScheduledJobs
{
    public class WeatherSyncScheduledJob
        : BaseScheduledJob
    {
        //private WeatherType _lastTickWeather;

        public WeatherType CurrentWeather { get; set; }

        public WeatherSyncScheduledJob()
            : base(TimeSpan.FromMinutes(15d))
        {
            Alt.OnPlayerConnect += (player, reason) => OnPlayerConnect(player as ServerPlayer, reason);
        }

        public override void Action()
        {
            foreach (var player in Alt.GetAllPlayers())
            {
                // if weather transitions not working correctly, check _lastTickWeather != CurrentWeather
                // and then emit event to all clients, which calls in a loop (0 to 100):
                // natives.setWeatherTypeTransition(alt.hash(oldWeather), alt.hash(currentWeather), loopStep);

                player.SetWeather((uint)CurrentWeather);

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
            }
        }

        private void OnPlayerConnect(ServerPlayer player, string reason)
        {
            player.SetWeather((uint)CurrentWeather);
        }
    }
}
