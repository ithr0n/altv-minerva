using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minerva.Server.DataAccessLayer.Context;
using Minerva.Server.ServerJobs.Base;
using Minerva.Server.Extensions;
using System.IO;
using Minerva.Server.ScheduledJobs.Base;
using Minerva.Server.Entities.Factories;
using Minerva.Server.Contracts.Configuration;
using Minerva.Server.Contracts.ScriptStrategy;

namespace Minerva.Server
{
    public class Server
        : AsyncResource
    {
        private readonly ServiceProvider _serviceProvider;

        public IConfiguration Configuration { get; }

        public Server()
            : base(new ActionTickSchedulerFactory())
        {
            // read and build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .Build();

            // initialize dependency injection
            var services = new ServiceCollection();
            
            // register configuration options
            services.Configure<GameOptions>(Configuration.GetSection(nameof(GameOptions)));
            services.Configure<DevelopmentOptions>(Configuration.GetSection(nameof(DevelopmentOptions)));

            // configure and register loggers
            services.AddLogging(config => config
                .AddConfiguration(Configuration.GetSection("Logging"))
                .AddDebug()
                .AddConsole());

            // register factory for database context
            services.AddDbContextFactory<DatabaseContext>(options => options
                .UseMySql(Configuration.GetConnectionString("Database"), MariaDbServerVersion.LatestSupportedServerVersion));

            // register all transient implementations
            services.AddAllTypes<ITransientScript>();

            // register all singleton implementations
            services.AddAllTypes<ISingletonScript>(ServiceLifetime.Singleton);

            // register all server jobs
            services.AddAllTypes<IServerJob>();

            // register all scheduled jobs
            services.AddAllTypes<ScheduledJob>();

            //
            // TODO implement something like validation for registered services, so that one service is not registered multiple times / with multiple lifetimes
            //

            // build DI services
            _serviceProvider = services.BuildServiceProvider();

            // everything done
            var _logger = _serviceProvider.GetService<ILogger<Server>>();
            _logger.LogDebug("Dependency Injection initialized successfully");
        }

        public override void OnStart()
        {
            // instanciate startup scripts
            _serviceProvider.InstanciateStartupScripts();

            // instanciate serverjobs
            var serverJobs = _serviceProvider.GetServices<IServerJob>();

            // execute startup handler of all server jobs
            foreach (var job in serverJobs)
            {
                job.OnStartup();
            }

            // instanciate scheduled jobs and enable them
            var scheduledJobsManager = _serviceProvider.GetService<ScheduledJobManager>();
            if (scheduledJobsManager != null)
            {
                scheduledJobsManager.EnableWorker();
            }
        }

        public override void OnTick()
        {
            base.OnTick();
        }

        public override void OnStop()
        {
            // cancel all scheduled jobs
            var scheduledJobsManager = _serviceProvider.GetService<ScheduledJobManager>();
            if (scheduledJobsManager != null)
            {
                scheduledJobsManager.Cancellation.Cancel();
            }

            // execute shutdown handler of all server jobs
            var serverJobs = _serviceProvider.GetServices<IServerJob>();
            foreach (var job in serverJobs)
            {
                job.OnShutdown();
            }
        }

        #region Entities

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new ServerPlayerFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new ServerVehicleFactory();
        }

        #endregion
    }
}
