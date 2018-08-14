using System.Collections.Generic;
using System.Threading.Tasks;
using IpcWrapper.Configurations;
using IpcWrapper.Exceptions;

namespace IpcWrapper.Factories
{
    public abstract class ServerFactory : IIpcServerFactory
    {
        protected IEnumerable<IIpcServerNameValidator> nameValidators;

        public ServerFactory() { }

        public ServerFactory(IEnumerable<IIpcServerNameValidator> nameValidators)
        {
            this.nameValidators = nameValidators;
        }

        public async Task<IIpcServer> CreateServerAsync(IIpcServerConfiguration configuration)
        {
            InternalValidateConfiguration(configuration);

            if (nameValidators != null)
            {
                ValidateServerName(configuration);
            }

            return await CreateTypedServerAsync(configuration);
        }

        protected virtual void ValidateServerName(IIpcServerConfiguration configuration)
        {
            try
            {
                Parallel.ForEach(nameValidators, validator =>
                {

                    validator.Validate(configuration.Name);
                });
            }
            catch (System.AggregateException ex)
            {
                throw new IpcServerNameInvalidException(configuration.Name, ex.InnerException);
            }
        }

        public virtual void ValidateConfiguration(IIpcServerConfiguration configuration)
        {
            if (configuration.Security == null)
            {
                throw new IpcServerConfigurationException("Server must have security setup");
            }
        }

        protected abstract Task<IIpcServer> CreateTypedServerAsync(IIpcServerConfiguration configuration);

        private void InternalValidateConfiguration(IIpcServerConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration.Name))
            {
                throw new IpcServerConfigurationException("Server must have a name set");
            }

            ValidateConfiguration(configuration);
        }
    }
}