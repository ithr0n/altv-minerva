using PlayGermany.Server.DataAccessLayer.Enums;
using PlayGermany.Server.DataAccessLayer.Models;
using PlayGermany.Server.Entities;

namespace PlayGermany.Server.ItemImplementations.Base
{
    public abstract class ItemImplementation
    {
        public DataAccessLayer.Enums.ItemImplementationType Type { get; }

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
