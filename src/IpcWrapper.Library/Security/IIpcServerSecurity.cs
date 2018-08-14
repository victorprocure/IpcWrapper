using System.Collections.Generic;
using System.Security.Principal;

namespace IpcWrapper.Security
{
    public interface IIpcServerSecurity
    {
        int MaxConnections { get; }

        IList<IIdentity> Users { get; }

        IpcSecurityAccessRights AccessRight { get; }
    }
}