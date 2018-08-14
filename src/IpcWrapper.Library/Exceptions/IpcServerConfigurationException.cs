namespace IpcWrapper.Exceptions
{
    public class IpcServerConfigurationException : System.Exception
    {
        public IpcServerConfigurationException() : this("Server Configuration invalid")
        {

        }
        
        public IpcServerConfigurationException(string message) : base(message) { }
    }
}