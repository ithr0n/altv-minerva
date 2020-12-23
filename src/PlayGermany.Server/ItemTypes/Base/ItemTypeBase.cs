using PlayGermany.Server.DataAccessLayer.Enums;
using PlayGermany.Server.DataAccessLayer.Models;
using PlayGermany.Server.Entities;

namespace PlayGermany.Server.ItemTypes.Base
{
    public abstract class ItemTypeBase
    {
        public ItemType Type { get; }

        public virtual bool OnBeforeUsed(ServerPlayer player)
        {
            return true;
        }

        public virtual void OnAfterUsed(ServerPlayer player)
        {
        }

        public virtual bool OnBeforeMoved(Inventory from, Inventory to)
        {
            return true;
        }

        public virtual void OnAfterMoved(Inventory from, Inventory to)
        {
        }

        public virtual void OnRemoved()
        {
        }
    }
}
