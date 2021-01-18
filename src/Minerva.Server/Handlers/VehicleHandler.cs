using AltV.Net;
using Minerva.Server.Core.Contracts.Abstractions.ScriptStrategy;
using Minerva.Server.Core.Contracts.Enums;
using Minerva.Server.Core.Entities;

namespace Minerva.Server.Handlers
{
    public class VehicleHandler
        : IStartupSingletonScript
    {
        public VehicleHandler()
        {
            Alt.OnClient<ServerPlayer, int>("Vehicle:ToggleIndicator", OnToggleIndicator);
            Alt.OnClient<ServerPlayer>("Vehicle:ToggleSiren", OnToggleSiren);
            Alt.OnClient<ServerPlayer, int>("Vehicle:RadioChanged", OnRadioChanged);
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
