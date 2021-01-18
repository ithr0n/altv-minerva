using System.Threading.Tasks;

namespace Minerva.Server.Core.Contracts.Abstractions
{
    public interface IServerJob
    {
        Task OnStartup();

        Task OnSave();

        Task OnShutdown();
    }
}
