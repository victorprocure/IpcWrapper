using System;

namespace IpcWrapper
{
    public interface IIpcCommunicator : IDisposable
    {
        bool CanReceiveCommunication { get; }

        bool CanSendCommunication { get; }
    }
}