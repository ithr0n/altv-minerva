using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.DataAccessLayer.Context;
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
