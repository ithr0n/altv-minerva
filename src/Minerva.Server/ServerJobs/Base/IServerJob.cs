using System.Threading.Tasks;

namespace Minerva.Server.ServerJobs.Base
{
    public interface IServerJob
    {
        void OnStartup();

        Task OnSave();

        void OnShutdown();
    }
}
