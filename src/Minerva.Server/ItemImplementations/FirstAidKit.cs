using Minerva.Server.Entities;
using Minerva.Server.ItemImplementations.Base;

namespace Minerva.Server.ItemImplementations
{
    public class FirstAidKit
        : ConsumableItemImplementation
    {
        public FirstAidKit()
            : base(1)
        {
        }

        public override void OnAfterUsed(ServerPlayer player)
        {
            player.Health += 30;

            base.OnAfterUsed(player);
        }
    }
}