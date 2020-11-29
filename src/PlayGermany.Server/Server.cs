using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.Entities;
using PlayGermany.Server.ServerJobs;
using PlayGermany.Server.ServerJobs.Base;
using PlayGermany.Server.Extensions;
using System.IO;
using System.Timers;
using PlayGermany.Server.Handlers;

namespace PlayGermany.Server
{
    public class Server
        : AsyncResource
    {
        private readonly ServiceProvider _serviceProvider;

        public IConfiguration Configuration { get; }

        public ILogger<Server> Logger { get; }

        public Server()
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration);

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

            // initialize world save
            var saveInterval = 1000 * 60 * 5; // default 5 mins

            if (!int.TryParse(Configuration.GetSection("World")["SaveInterval"], out saveInterval))
            {
                Logger.LogWarning("No world save interval configured in appsettings.json, taking fallback value of 5 mins.");
            }

            var timer = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = saveInterval
            };

            timer.Elapsed += TimerWorldSaveElapsed;
        }

        public override void OnTick()
        {
            base.OnTick();
        }

        public override void OnStop()
        {
            var serverJobs = _serviceProvider.GetServices<IServerJob>();
            foreach (var job in serverJobs)
            {
                job.OnShutdown();
            }
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });

            services.AddEntityFrameworkMySql();
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("Database"), MariaDbServerVersion.LatestSupportedServerVersion);
            });

            // register for DI below
            services.RegisterAllTypes<IServerJob>(new[] { typeof(Server).Assembly });
            services.RegisterScopedAndInstanciate<SessionHandler>();
        }

        private void TimerWorldSaveElapsed(object sender, ElapsedEventArgs e)
        {
            var serverJobs = _serviceProvider.GetServices<IServerJob>();
            foreach (var job in serverJobs)
            {
                job.OnSave();
            }

            Logger.LogDebug("World save at {CurrentDate}", System.DateTime.Now);
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
