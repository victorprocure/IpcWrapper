using System;
using System.Threading;
using System.Threading.Tasks;
using IpcWrapper.Events;
using Moq;
using Xunit;

namespace IpcWrapper.Library.Tests
{
    public class IpcServerTests
    {
        [Fact]
        public async Task ServerShouldExecuteEventOnClientConnection()
        {
            var server = new TestServer();
            var iterationCount = (int)Math.Floor((double)TestServer.IterationCount / 2d);

            var iteration = 0;
            server.ClientConnected += (o, e) => ++iteration;
            await server.InitializeAsync();

            Assert.Equal(iterationCount, iteration);
        }

        [Fact]
        public async Task ServerShouldRaiseEventOnClientDisconnect()
        {
            var server = new TestServer();
            var iterationCount = (int)Math.Floor((double)TestServer.IterationCount / 3d);
            var iteration = 0;
            server.ClientDisconnected += (o, e) => ++iteration;
            await server.InitializeAsync();

            Assert.Equal(iterationCount, iteration);
        }

        [Fact]
        public async Task ServerShouldExecuteEventOnInitialization()
        {
            var serverMock = new TestServer();
            var raisedEvent = false;
            serverMock.Initialized += (o, e) =>
            {
                raisedEvent = true;
            };

            await serverMock.InitializeAsync();

            Assert.True(raisedEvent);
        }

        private class TestServer : IpcServer
        {
            internal const int IterationCount = 10;

            public override bool CanReceiveCommunication => false;

            public override bool CanSendCommunication => false;

            public override event EventHandler<IpcServerClientConnectedEventArgs> ClientConnected;
            public override event EventHandler<IpcServerClientDisconnectedEventArgs> ClientDisconnected;

            private int iteration = 1;

            public override void Dispose()
            {
                throw new System.NotImplementedException();
            }

            public override async Task StartServerAsync()
            {
                await Task.CompletedTask;
            }

            public override async Task WaitForConnectionsAsync(CancellationToken token)
            {
                if (iteration % 2 == 0)
                {
                    if (ClientConnected != null)
                    {
                        ClientConnected.Invoke(this, new IpcServerClientConnectedEventArgs());
                    }
                }

                if (iteration % 3 == 0)
                {
                    if (ClientDisconnected != null)
                    {
                        ClientDisconnected.Invoke(this, new IpcServerClientDisconnectedEventArgs());
                    }
                }

                if (iteration < IterationCount)
                {
                    Interlocked.Increment(ref iteration);
                }
                else
                {
                    this.Cancel();
                }

                await Task.CompletedTask;
            }
        }
    }
}