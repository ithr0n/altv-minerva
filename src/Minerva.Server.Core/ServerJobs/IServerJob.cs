using System.Threading.Tasks;

namespace Minerva.Server.Core.ServerJobs
{
    public interface IServerJob
    {
        Task OnStartup();

        Task OnSave();

        Task OnShutdown();
    }
}
