using System;
using Minerva.Server.Entities;
using Minerva.Server.PlayerBuffs.Base;

namespace Minerva.Server.PlayerBuffs
{
    public class DemoBuff
        : PlayerBuff
    {
        private int _maxHealthModifier;

        public DemoBuff(int maxHealthModifier)
            : base(TimeSpan.Zero)
        {
            _maxHealthModifier = maxHealthModifier;
        }

        public override void OnApplied(ServerPlayer player)
        {
            player.MaxHealth = (ushort)Math.Max(player.MaxHealth + _maxHealthModifier, 0);
        }

        public override void OnRemoved(ServerPlayer player)
        {
            player.MaxHealth = (ushort)Math.Max(player.MaxHealth - _maxHealthModifier, 0);
        }
    }
}