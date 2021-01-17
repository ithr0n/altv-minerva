using Minerva.Server.Core.Entities;

namespace Minerva.Server.ItemImplementations.Base
{
    public abstract class ConsumableItemImplementation
        : ItemImplementation
    {
        public int Durability { get; set; }

        public ConsumableItemImplementation(int durability)
        {
            Durability = durability;
        }

        public override void OnAfterUsed(ServerPlayer player)
        {
            // ...
        }
    }
}