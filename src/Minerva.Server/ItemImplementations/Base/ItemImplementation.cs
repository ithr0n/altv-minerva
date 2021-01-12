using Minerva.Server.DataAccessLayer.Enums;
using Minerva.Server.DataAccessLayer.Models;
using Minerva.Server.Entities;

namespace Minerva.Server.ItemImplementations.Base
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
