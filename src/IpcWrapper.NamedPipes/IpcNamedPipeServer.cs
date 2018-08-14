using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using IpcWrapper.Configurations;
using IpcWrapper.Events;

namespace IpcWrapper.NamedPipes
{
    public class IpcNamedPipeServer : IpcServer
    {
        public override bool CanReceiveCommunication => true;

        public override bool CanSendCommunication => true;

        public override event EventHandler<IpcServerClientConnectedEventArgs> ClientConnected;
        public override event EventHandler<IpcServerClientDisconnectedEventArgs> ClientDisconnected;

        private int nextPipeId;

        internal IpcNamedPipeServer(IIpcServerConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override async Task StartServerAsync()
        {
            var connectionPipeName = GetNextConnectionPipeName();

            try
            {
                var handshakePipe = await GetHandshakePipe();
                var handshakeHandler = new NamedPipeObjectWriter<string>(handshakePipe);
                await handshakeHandler.WriteObject(connectionPipeName);
                handshakePipe.WaitForPipeDrain();
                handshakePipe.Close();

                var connectionPipe = CreatePipe(connectionPipeName);
                await connectionPipe.WaitForConnectionAsync();

                var connection = 
            }
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override Task WaitForConnectionsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        private string GetNextConnectionPipeName()
        {
            return string.Format("{0}_{1}", Configuration.Name, nextPipeId++);
        }

        private static NamedPipeServerStream CreatePipe(string pipeName)
        {
            var pipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.WriteThrough, 0, 0);
            return pipe;
        }
        private async Task<NamedPipeServerStream> GetHandshakePipe()
        {
            var pipe = CreatePipe(Configuration.Name);
            await pipe.WaitForConnectionAsync();

            return pipe;
        }
    }
}