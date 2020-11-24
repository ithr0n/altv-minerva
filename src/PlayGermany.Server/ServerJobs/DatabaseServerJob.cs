using Microsoft.Extensions.Logging;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.ServerJobs.Base;

namespace PlayGermany.Server.ServerJobs
{
    public class DatabaseServerJob
        : IServerJob
    {
        private readonly DatabaseContext _dbContext;

        private ILogger<DatabaseServerJob> Logger { get; }

        public DatabaseServerJob(
            DatabaseContext dbContext,
            ILogger<DatabaseServerJob> logger)
        {
            Logger = logger;

            _dbContext = dbContext;
        }

        public void OnSave()
        {

        }

        public void OnShutdown()
        {

        }

        public void OnStartup()
        {
            _dbContext.Database.EnsureDeleted();
            Logger.LogWarning("Database dropped");

            _dbContext.Database.EnsureCreated();
            Logger.LogInformation("Database created");
        }
    }
}
