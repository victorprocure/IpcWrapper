using System.Collections.Generic;
using IpcWrapper.Factories;
using IpcWrapper.Security;

namespace IpcWrapper.Configurations
{
    public interface IIpcServerConfiguration : IIpcConfiguration
    {
        IIpcServerSecurity Security { get; }
    }
}