using System;
using PlayGermany.Server.Entities;

namespace PlayGermany.Server.PlayerBuffs.Base
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