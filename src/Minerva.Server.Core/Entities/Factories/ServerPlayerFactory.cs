using AltV.Net;
using AltV.Net.Elements.Entities;
using System;

namespace Minerva.Server.Core.Entities.Factories
{
    public class ServerPlayerFactory
        : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IntPtr entityPointer, ushort id)
        {
            return new ServerPlayer(entityPointer, id);
        }
    }
}
