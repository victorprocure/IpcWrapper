using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace IpcWrapper.Security
{
    public sealed class IpcDefaultServerSecurity : IIpcServerSecurity
    {
        public int MaxConnections { get; }

        public IList<IIdentity> Users { get; }

        public IpcSecurityAccessRights AccessRight { get; }

        public IpcDefaultServerSecurity()
        {
            MaxConnections = 1;
            Users = new List<IIdentity>
            {
                new GenericIdentity($"{Environment.UserDomainName}\\{Environment.UserName}", IpcAuthenticationTypes.Anonymous)
            };

            AccessRight = IpcSecurityAccessRights.ReadWrite;
        }
    }
}