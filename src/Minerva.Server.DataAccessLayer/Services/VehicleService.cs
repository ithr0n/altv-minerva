using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minerva.Server.Core.Contracts.Models;
using Minerva.Server.Core.ScriptStrategy;
using Minerva.Server.DataAccessLayer.Context;

namespace Minerva.Server.DataAccessLayer.Services
{
    public class VehicleService
        : ITransientScript
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public VehicleService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Vehicle> CreateNewVehicle(Vehicle vehicle, float vehicleInventoryMaxWeight, Item keyItem)
        {
            if (vehicle == null || vehicle.Id != 0)
            {
                throw new InvalidOperationException("Vehicle invalid");
            }

            if (keyItem == null)
            {
                throw new InvalidOperationException("KeyItem invalid.");
            }

            using var dbContext = _dbContextFactory.CreateDbContext();

            vehicle.Inventory = new Inventory
            {
                MaxWeight = vehicleInventoryMaxWeight
            };

            vehicle.KeyData = new KeyData
            {
                Item = keyItem
            };

            dbContext.Vehicles.Add(vehicle);
            await dbContext.SaveChangesAsync();

            return vehicle;
        }
    }
}
