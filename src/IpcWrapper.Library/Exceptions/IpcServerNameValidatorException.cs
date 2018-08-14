using System;

namespace IpcWrapper.Exceptions
{
    public class IpcServerNameValidatorException : Exception
    {
        public IpcServerNameValidatorException(string message) : base(message) { 

        }
    }
}