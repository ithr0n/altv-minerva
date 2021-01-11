using AltV.Net;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Entities;
using PlayGermany.Server.Enums;

namespace PlayGermany.Server.Handlers
{
    public class VehicleHandler
    {
        private ILogger<VehicleHandler> Logger { get; }

        public VehicleHandler(ILogger<VehicleHandler> logger)
        {
            Alt.OnClient<ServerPlayer, int>("Vehicle:ToggleIndicator", OnToggleIndicator);
            Alt.OnClient<ServerPlayer>("Vehicle:ToggleSiren", OnToggleSiren);
            Alt.OnClient<ServerPlayer, int>("Vehicle:RadioChanged", OnRadioChanged);

            Logger = logger;
        }

        private void OnToggleIndicator(ServerPlayer player, int indicatorFlag)
        {
            if (player.IsInVehicle && player.Seat == 1)
            {
                var veh = (ServerVehicle)player.Vehicle;

                if (veh.ActiveIndicators == (VehicleIndicator)indicatorFlag)
                {
                    veh.ActiveIndicators = VehicleIndicator.None;
                }
                else
                {
                    veh.ActiveIndicators = (VehicleIndicator)indicatorFlag;
                }
            }
        }

        private void OnToggleSiren(ServerPlayer player)
        {
            if (player.IsInVehicle)
            {
                if (!player.Vehicle.GetStreamSyncedMetaData("sirenDisabled", out bool oldState))
                {
                    oldState = false;
                }

                player.Vehicle.SetStreamSyncedMetaData("sirenDisabled", !oldState);
            }
        }

        private void OnRadioChanged(ServerPlayer player, int radioStationIndex)
        {
            if (player.IsInVehicle)
            {
                player.Vehicle.SetStreamSyncedMetaData("radioStation", radioStationIndex);
            }
        }
    }
}
