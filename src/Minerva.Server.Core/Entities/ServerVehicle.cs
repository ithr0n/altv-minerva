using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Minerva.Server.Core.Contracts.Enums;
using System;

namespace Minerva.Server.Core.Entities
{
    public class ServerVehicle
        : Vehicle
    {
        public ServerVehicle(IntPtr nativePointer, ushort id)
            : base(nativePointer, id)
        {
        }

        public ServerVehicle(uint model, Position position, Rotation rotation)
            : base(model, position, rotation)
        {
        }

        public VehicleIndicator ActiveIndicators
        {
            get
            {
                if (!GetStreamSyncedMetaData("indicators", out int result))
                {
                    return VehicleIndicator.None;
                }

                return (VehicleIndicator)result;
            }
            set
            {
                SetStreamSyncedMetaData("indicators", (int)value);
            }
        }

        public Contracts.Models.Vehicle DbEntity { get; set; }
    }
}