using AltV.Net;
using AltV.Net.Elements.Entities;
using System;

namespace PlayGermany.Server.Entities
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
