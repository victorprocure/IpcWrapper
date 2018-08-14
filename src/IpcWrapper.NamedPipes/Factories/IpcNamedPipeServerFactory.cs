using System.Threading.Tasks;
using IpcWrapper.Configurations;
using IpcWrapper.Factories;
using IpcWrapper.NamedPipes.Validators;

namespace IpcWrapper.NamedPipes.Factories
{
    public class IpcNamedPipeServerFactory : ServerFactory
    {
        public IpcNamedPipeServerFactory() : base(new[] { new ServerNameHasNoSpacesValidator() })
        {
        }

        protected override Task<IIpcServer> CreateTypedServerAsync(IIpcServerConfiguration configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}