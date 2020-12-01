using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;

namespace PlayGermany.Server.Entities
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
    }
}