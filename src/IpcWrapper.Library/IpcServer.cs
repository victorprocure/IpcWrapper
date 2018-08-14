using System;
using System.Threading;
using System.Threading.Tasks;
using IpcWrapper.Configurations;
using IpcWrapper.Events;

namespace IpcWrapper
{
    public abstract class IpcServer : IIpcServer
    {
        public abstract event EventHandler<IpcServerClientConnectedEventArgs> ClientConnected;

        public abstract event EventHandler<IpcServerClientDisconnectedEventArgs> ClientDisconnected;

        public event EventHandler<IpcServerInitializedEventArgs> Initialized;

        public IIpcServerConfiguration Configuration { get; protected set;}

        public abstract bool CanReceiveCommunication { get; }

        public abstract bool CanSendCommunication { get; }

        public bool CanReceiveConnections { get; protected set; }

        private static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        protected CancellationToken CancellationToken = CancellationTokenSource.Token;

        public IpcServer()
        {
        }

        private void OnInitialized()
        {
            var handler = Initialized;
            if (handler != null)
            {
                handler.Invoke(this, new IpcServerInitializedEventArgs());
            }
        }

        private async Task BeginListenerAsync()
        {
            while (true)
            {
                try
                {
                    CancellationToken.ThrowIfCancellationRequested();
                    await WaitForConnectionsAsync(CancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        protected virtual void Cancel()
        {
            CancellationTokenSource.Cancel();
        }

        public virtual async Task InitializeAsync()
        {
            await StartServerAsync();
            OnInitialized();

            await BeginListenerAsync();
        }

        public abstract Task StartServerAsync();

        public abstract Task WaitForConnectionsAsync(CancellationToken cancellationToken = default(CancellationToken));

        public abstract void Dispose();
    }
}