using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minerva.Server.Core.Contracts.Abstractions.ScriptStrategy;
using Minerva.Server.Core.Contracts.Models.Database;
using Minerva.Server.DataAccessLayer.Context;

namespace Minerva.Server.DataAccessLayer.Services
{
    public class InventoryService
        : ITransientScript
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public InventoryService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Inventory> LoadInventoryWithItems(int inventoryId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var inventory = await dbContext.Inventories.Include(e => e.Items).SingleOrDefaultAsync(e => e.Id == inventoryId);

            return inventory;
        }

        public async Task<Inventory> CreateNew()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var inventory = new Inventory();

            dbContext.Inventories.Add(inventory);
            await dbContext.SaveChangesAsync();

            return inventory;
        }

        public float GetAvailableCapacity(Inventory inventory)
        {
            return 0f;
        }

        public bool AddNewItemToInventory(Inventory inventory, Item item)
        {
            return false;
        }

        public bool RemoveItemFromInventory(Inventory inventory, Item item)
        {
            return false;
        }
    }
}