using System.Threading.Tasks;

namespace PlayGermany.Server.ServerJobs.Base
{
    public interface IServerJob
    {
        void OnStartup();

        Task OnSave();

        void OnShutdown();
    }
}
