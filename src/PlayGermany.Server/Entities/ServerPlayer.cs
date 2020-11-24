using AltV.Net.Elements.Entities;
using System;

namespace PlayGermany.Server.Entities
{
    internal class ServerPlayer : Player
    {
        public ServerPlayer(IntPtr nativePointer, ushort id) 
            : base(nativePointer, id)
        {
        }
    }
}