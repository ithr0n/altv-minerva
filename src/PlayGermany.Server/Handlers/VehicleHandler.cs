using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Entities;
using PlayGermany.Server.Enums;

namespace PlayGermany.Server.Handlers
{
    public class VehicleHandler
    {
        public ILogger<VehicleHandler> Logger { get; }

        public VehicleHandler(ILogger<VehicleHandler> logger)
        {
            Alt.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
            Alt.OnPlayerLeaveVehicle += OnPlayerLeaveVehicle;
            Alt.OnPlayerChangeVehicleSeat += OnPlayerChangeVehicleSeat;
            Alt.OnClient<ServerPlayer, int>("Vehicle:ToggleIndicator", OnToggleIndicator);
            Alt.OnClient<ServerPlayer>("Vehicle:ToggleSiren", OnToggleSiren);

            Logger = logger;
        }

        private void OnPlayerEnterVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            player.Emit("playerEnteredVehicle", vehicle, (int)seat);
        }

        private void OnPlayerLeaveVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            player.Emit("playerLeftVehicle", vehicle, (int)seat);
        }

        private void OnPlayerChangeVehicleSeat(IVehicle vehicle, IPlayer player, byte oldSeat, byte newSeat)
        {
            player.Emit("playerChangeVehicleSeat", vehicle, (int)oldSeat, (int)newSeat);
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
    }
}
