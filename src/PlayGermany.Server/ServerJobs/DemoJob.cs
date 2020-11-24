using PlayGermany.Server.ServerJobs.Base;
using System;

namespace PlayGermany.Server.ServerJobs
{
    public class DemoJob
        : IServerJob
    {
        public void OnSave()
        {
            Console.WriteLine("OnSave Demo");
        }

        public void OnShutdown()
        {
            Console.WriteLine("OnShutdown Demo");
        }

        public void OnStartup()
        {
            Console.WriteLine("OnStartup Demo");
        }
    }
}
