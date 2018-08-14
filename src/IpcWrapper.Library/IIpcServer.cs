using System;
using System.Threading;
using System.Threading.Tasks;
using IpcWrapper.Configurations;
using IpcWrapper.Events;
using IpcWrapper.Security;

namespace IpcWrapper
{
    public interface IIpcServer : IIpcCommunicator
    {
        event EventHandler<IpcServerClientConnectedEventArgs> ClientConnected;
        event EventHandler<IpcServerClientDisconnectedEventArgs> ClientDisconnected;

        event EventHandler<IpcServerInitializedEventArgs> Initialized;

        IIpcServerConfiguration Configuration { get; }

        bool CanReceiveConnections { get; }

        Task StartServerAsync();

        Task WaitForConnectionsAsync(CancellationToken cancellationToken);
    }
}