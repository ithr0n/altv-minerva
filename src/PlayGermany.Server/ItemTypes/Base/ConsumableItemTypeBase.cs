using System;
using PlayGermany.Server.Entities;

namespace PlayGermany.Server.ItemTypes.Base
{
    public abstract class ConsumableItemTypeBase
        : ItemTypeBase
    {
        private readonly int _hungerModifier;
        private readonly int _thirstModifier;
        private readonly int _alcoholModifier;
        private readonly int _drugsModifier;

        public ConsumableItemTypeBase(
            int hungerModifier = 0, 
            int thirstModifier = 0, 
            int alcoholModifier = 0, 
            int drugsModifier = 0)
        {
            _hungerModifier = hungerModifier;
            _thirstModifier = thirstModifier;
            _alcoholModifier = alcoholModifier;
            _drugsModifier = drugsModifier;
        }

        public override void OnAfterUsed(ServerPlayer player)
        {
            player.Hunger = (ushort) Math.Max(player.Hunger + _hungerModifier, 0);
            player.Thirst = (ushort) Math.Max(player.Thirst + _thirstModifier, 0);
            // player.Alcohol = (ushort) Math.Max(player.Alcohol + _alcoholModifier, 0);
            // player.Drugs = (ushort) Math.Max(player.Drugs + _drugsModifier, 0);
        }
    }
}