namespace PlayGermany.Server.ServerJobs.Base
{
    public interface IServerJob
    {
        void OnStartup();

        void OnSave();

        void OnShutdown();
    }
}
