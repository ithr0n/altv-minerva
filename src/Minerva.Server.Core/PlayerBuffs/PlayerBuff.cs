using System;
using Minerva.Server.Core.Entities;

namespace Minerva.Server.Core.PlayerBuffs
{
    public abstract class PlayerBuff
    {
        public TimeSpan TickInterval { get; }

        public DateTime AppliedAt { get; set; }

        public PlayerBuff(TimeSpan tickInterval)
        {
            TickInterval = tickInterval;
        }

        public virtual void OnApplied(ServerPlayer player)
        {
        }

        public virtual void OnRemoved(ServerPlayer player)
        {
        }

        public virtual void OnTick(ServerPlayer player)
        {
        }
    }
}