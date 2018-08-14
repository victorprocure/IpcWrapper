using System.Collections.Generic;
using IpcWrapper.Factories;
using IpcWrapper.Security;

namespace IpcWrapper.Configurations
{
    public class IpcServerConfiguration : IIpcServerConfiguration
    {
        public IIpcServerSecurity Security
        {
            get;
        }

        public string Name { get; }

        public IpcServerConfiguration(string serverName, IIpcServerSecurity security)
        {
            Security = security;
            Name = serverName;
        }

        public IpcServerConfiguration(string serverName) : this(serverName, new IpcDefaultServerSecurity())
        {
        }
    }
}