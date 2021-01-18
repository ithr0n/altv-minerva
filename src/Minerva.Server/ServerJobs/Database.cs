using System.Collections.Generic;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minerva.Server.Core.Callbacks;
using Minerva.Server.Core.Configuration;
using Minerva.Server.Core.Contracts.Abstractions;
using Minerva.Server.Core.Contracts.Enums;
using Minerva.Server.Core.Contracts.Models.Database;
using Minerva.Server.Core.Entities;
using Minerva.Server.DataAccessLayer.Context;

namespace Minerva.Server.ServerJobs
{
    public class Database
        : IServerJob
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
        private readonly DevelopmentOptions _devOptions;

        private ILogger<Database> Logger { get; }

        public Database(
            IDbContextFactory<DatabaseContext> dbContextFactory,
            IOptions<DevelopmentOptions> devOptions,
            ILogger<Database> logger)
        {
            Logger = logger;

            _dbContextFactory = dbContextFactory;
            _devOptions = devOptions.Value;
        }

        public Task OnSave()
        {
            // characters
            var playersTask = Task.Run(async () =>
            {
                var charsToUpdate = new List<Character>();
                var callback = new AsyncFunctionCallback<IPlayer>(async (player) =>
                {
                    var serverPlayer = (ServerPlayer)player;

                    if (serverPlayer.IsSpawned)
                    {
                        serverPlayer.Character.Position = serverPlayer.Position;
                        serverPlayer.Character.Rotation = serverPlayer.Rotation;

                        serverPlayer.Character.Health = serverPlayer.Health;
                        serverPlayer.Character.Armor = serverPlayer.Armor;

                        serverPlayer.Character.Cash = serverPlayer.Cash;
                        serverPlayer.Character.Thirst = serverPlayer.Thirst;
                        serverPlayer.Character.Hunger = serverPlayer.Hunger;
                        // serverPlayer.Character.Alcohol = serverPlayer.Alcohol;
                        // serverPlayer.Character.Drugs = serverPlayer.Drugs;

                        charsToUpdate.Add(serverPlayer.Character);
                    }

                    await Task.CompletedTask;
                });

                await Alt.ForEachPlayers(callback);

                using var dbContext = _dbContextFactory.CreateDbContext();
                dbContext.Characters.UpdateRange(charsToUpdate);
                await dbContext.SaveChangesAsync();
            });

            var vehiclesTask = Task.Run(async () =>
            {
                var vehiclesToUpdate = new List<Core.Contracts.Models.Database.Vehicle>();
                var callback = new AsyncFunctionCallback<IVehicle>(async (vehicle) =>
                {
                    var serverVehicle = (ServerVehicle)vehicle;

                    if (serverVehicle.DbEntity != null)
                    {
                        serverVehicle.DbEntity.Position = serverVehicle.Position;
                        serverVehicle.DbEntity.Rotation = serverVehicle.Rotation;

                        serverVehicle.DbEntity.Locked = serverVehicle.LockState != AltV.Net.Enums.VehicleLockState.Unlocked;
                        // serverVehicle.DbEntity.Fuel = serverVehicle.Fuel;
                        // serverVehicle.DbEntity.Mileage = serverVehicle.Mileage;

                        vehiclesToUpdate.Add(serverVehicle.DbEntity);
                    }

                    await Task.CompletedTask;
                });

                await Alt.ForEachVehicles(callback);

                using var dbContext = _dbContextFactory.CreateDbContext();
                dbContext.Vehicles.UpdateRange(vehiclesToUpdate);
                await dbContext.SaveChangesAsync();
            });

            return Task.WhenAll(playersTask, vehiclesTask);
        }

        public async Task OnShutdown()
        {
            await Task.CompletedTask;
        }

        public async Task OnStartup()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            if (_devOptions.DropDatabaseAtStartup)
            {
                dbContext.Database.EnsureDeleted();
                Logger.LogWarning("Database dropped");
            }

            dbContext.Database.EnsureCreated();
            Logger.LogInformation("Database created");

            // seedings
            if (!await dbContext.Accounts.AnyAsync())
            {
                dbContext.Accounts.Add(new Account
                {
                    SocialClubId = 305176062,
                    AccessLevel = AccessLevel.Owner,
                    Password = "ee26b0dd4af7e749aa1a8ee3c10ae9923f618980772e473f8819a5d4940e0db27ac185f8a0e1d5f84f88bc887fd67b143732c304cc5fa9ad8e6f57f50028a8ff"
                });
            }

            dbContext.SaveChanges();
        }
    }
}
