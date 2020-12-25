using System.Collections.Generic;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Callbacks;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.DataAccessLayer.Models;
using PlayGermany.Server.Entities;
using PlayGermany.Server.ServerJobs.Base;

namespace PlayGermany.Server.ServerJobs
{
    public class DatabaseServerJob
        : IServerJob
        {
            private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

            private ILogger<DatabaseServerJob> Logger { get; }

            public DatabaseServerJob(
                IDbContextFactory<DatabaseContext> dbContextFactory,
                ILogger<DatabaseServerJob> logger)
            {
                Logger = logger;

                _dbContextFactory = dbContextFactory;
            }

            public void OnSave()
            {
                Task.Run(async() =>
                {
                    var charsToUpdate = new List<Character>();
                    var callback = new FunctionCallback<IPlayer>((player) =>
                    {
                        var serverPlayer = (ServerPlayer) player;

                        serverPlayer.Character.Health = serverPlayer.Health;
                        serverPlayer.Character.Armor = serverPlayer.Armor;
                        serverPlayer.Character.Position = serverPlayer.Position;
                        serverPlayer.Character.Cash = serverPlayer.Cash;
                        serverPlayer.Character.Thirst = serverPlayer.Thirst;
                        serverPlayer.Character.Hunger = serverPlayer.Hunger;
                        // serverPlayer.Character.Drugs = serverPlayer.Drugs;

                        charsToUpdate.Add(serverPlayer.Character);
                    });

                    Alt.ForEachPlayers(callback);

                    using var dbContext = _dbContextFactory.CreateDbContext();
                    dbContext.Characters.UpdateRange(charsToUpdate);
                    await dbContext.SaveChangesAsync();
                });
            }

            public void OnShutdown()
            {

            }

            public void OnStartup()
            {
                using var dbContext = _dbContextFactory.CreateDbContext();

                dbContext.Database.EnsureDeleted();
                Logger.LogWarning("Database dropped");

                dbContext.Database.EnsureCreated();
                Logger.LogInformation("Database created");
            }
        }
}
