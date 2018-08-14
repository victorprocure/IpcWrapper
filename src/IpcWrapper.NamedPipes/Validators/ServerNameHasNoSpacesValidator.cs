using IpcWrapper.Exceptions;
using IpcWrapper.Factories;

namespace IpcWrapper.NamedPipes.Validators
{
    public class ServerNameHasNoSpacesValidator : IIpcServerNameValidator
    {
        public void Validate(string serverName)
        {
            if(serverName.Contains(" ")){
                throw new IpcServerNameValidatorException("Server name contains a space");
            }
        }
    }
}