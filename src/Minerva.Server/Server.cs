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
using Minerva.Server.Handlers;
using Minerva.Server.ScheduledJobs.Base;
using Minerva.Server.Entities.Factories;
using Minerva.Server.EntitySync.Streamers;
using Minerva.Server.DataAccessLayer.Services;
using Minerva.Server.Contracts.Configuration;

namespace Minerva.Server
{
    public class Server
        : AsyncResource
    {
        private readonly ServiceProvider _serviceProvider;

        public IConfiguration Configuration { get; }

        public ILogger<Server> Logger { get; }

        public Server()
            : base(new ActionTickSchedulerFactory())
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .Build();

            var services = new ServiceCollection();
            
            services.Configure<GameOptions>(Configuration.GetSection(nameof(GameOptions)));
            services.Configure<DevelopmentOptions>(Configuration.GetSection(nameof(DevelopmentOptions)));

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            Logger = _serviceProvider.GetService<ILogger<Server>>();
        }

        public override void OnStart()
        {
            _serviceProvider.InstanciateRegisteredServices();

            var serverJobs = _serviceProvider.GetServices<IServerJob>();
            foreach (var job in serverJobs)
            {
                job.OnStartup();
            }

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
            var scheduledJobsManager = _serviceProvider.GetService<ScheduledJobManager>();
            if (scheduledJobsManager != null)
            {
                scheduledJobsManager.Cancellation.Cancel();
            }

            var serverJobs = _serviceProvider.GetServices<IServerJob>();
            foreach (var job in serverJobs)
            {
                job.OnShutdown();
            }
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(config => config
                .AddConfiguration(Configuration.GetSection("Logging"))
                .AddDebug()
                .AddConsole());

            services.AddDbContextFactory<DatabaseContext>(options => options
                .UseMySql(Configuration.GetConnectionString("Database"), MariaDbServerVersion.LatestSupportedServerVersion));

            /*services.AddScoped(p => p
                .GetRequiredService<IDbContextFactory<DatabaseContext>>()
                .CreateDbContext());*/

            // register all server and scheduled jobs
            services.AddAllTypes<IServerJob>();
            services.AddAllTypes<ScheduledJob>();
            services.AddSingletonAndInstanciate<ScheduledJobManager>();

            // register streamers
            services.AddSingleton<PropsStreamer>();
            services.AddSingleton<MarkersStreamer>();
            services.AddSingleton<StaticBlipsStreamer>();

            // register handlers
            services.AddSingletonAndInstanciate<ClientConsoleHandler>();
            services.AddSingletonAndInstanciate<ServerConsoleHandler>();
            services.AddSingletonAndInstanciate<SessionHandler>();
            services.AddSingletonAndInstanciate<VehicleHandler>();
            services.AddSingletonAndInstanciate<VoiceHandler>();

            // register services
            services.AddScoped<AccountService>();
            services.AddScoped<BankAccountService>();
            services.AddScoped<CharacterService>();

            // custom
            services.AddSingleton<WorldData>();
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
