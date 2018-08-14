using System.Threading.Tasks;
using IpcWrapper;
using IpcWrapper.Configurations;

namespace IpcWrapper.Factories
{
    public interface IIpcServerFactory
    {
         Task<IIpcServer> CreateServerAsync(IIpcServerConfiguration configuration);
    }
}