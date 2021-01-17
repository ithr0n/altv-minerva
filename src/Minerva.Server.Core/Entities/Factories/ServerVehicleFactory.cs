using AltV.Net;
using AltV.Net.Elements.Entities;
using System;

namespace Minerva.Server.Core.Entities.Factories
{
    public class ServerVehicleFactory
        : IEntityFactory<IVehicle>
    {
        public IVehicle Create(IntPtr entityPointer, ushort id)
        {
            return new ServerVehicle(entityPointer, id);
        }
    }
}
