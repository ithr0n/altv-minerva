using Ninject;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.ServerJobs.Base;
using System;
using System.Collections.Generic;
using System.Timers;

namespace PlayGermany.Server
{
    public class Kernel
        : StandardKernel
    {
        private Timer _worldSaveTimer;

        internal void Initialize()
        {
            // prepare world save timer
            _worldSaveTimer = new Timer()
            {
                Enabled = false,
                Interval = 1000 * 60 * 5,
            };
            _worldSaveTimer.Elapsed += WorldSave;

            // register bindings
        }

        private void WorldSave(object sender, ElapsedEventArgs e)
        {
            this.Get<List<IServerJob>>()
                .ForEach(job => job.OnSave());
        }

        public void Startup()
        {
            InitializeDatabase();

            this.Get<List<IServerJob>>()
                .ForEach(job => job.OnStartup());
        }

        public void Shutdown()
        {
            this.Get<List<IServerJob>>()
                .ForEach(job => job.OnSave());

            this.Get<List<IServerJob>>()
                .ForEach(job => job.OnShutdown());
        }

        private void InitializeDatabase()
        {
            using var context = new DatabaseContext("Server=localhost;Database=playgermany;Uid=playgermany;Pwd=playgermany;"); // todo: put in config file

            context.Database.EnsureDeleted(); // comment this out to disable auto delete database
            context.Database.EnsureCreated();

            Console.WriteLine("Database verified.");
        }
    }
}
