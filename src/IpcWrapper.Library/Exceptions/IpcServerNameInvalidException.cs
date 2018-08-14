using System;

namespace IpcWrapper.Exceptions
{
    public class IpcServerNameInvalidException : Exception
    {
        public IpcServerNameInvalidException(string serverName) : this(serverName, null)
        {

        }
        public IpcServerNameInvalidException(string serverName, Exception innerException) : base($"Invalid Server Name: {serverName}", innerException)
        {
        }
    }
}