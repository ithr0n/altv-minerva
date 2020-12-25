using System;
using PlayGermany.Server.Entities;
using PlayGermany.Server.PlayerBuffs.Base;

namespace PlayGermany.Server.PlayerBuffs
{
    public class DemoBuff
        : PlayerBuff
    {
        private int _maxHealthModifier;

        public DemoBuff(int maxHealthModifier)
            : base(TimeSpan.Zero)
        {
            
        }

        public override void OnApplied(ServerPlayer player)
        {
            player.MaxHealth = (ushort) Math.Max(player.MaxHealth + _maxHealthModifier, 0);
        }

        public override void OnRemoved(ServerPlayer player)
        {
            player.MaxHealth = (ushort) Math.Max(player.MaxHealth - _maxHealthModifier, 0);
        }
    }
}