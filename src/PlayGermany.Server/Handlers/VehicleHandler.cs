using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.Logging;

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
    }
}
