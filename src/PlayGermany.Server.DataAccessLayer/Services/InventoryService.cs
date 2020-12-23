using Microsoft.EntityFrameworkCore;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.DataAccessLayer.Models;

namespace PlayGermany.Server.DataAccessLayer.Services
{
    public class InventoryService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public InventoryService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory
        }

        public Inventory CreateNew()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var inventory = new Inventory();

            dbContext.Inventories.Add(inventory);
            dbContext.SaveChanges();

            return inventory;
        }
    }
}