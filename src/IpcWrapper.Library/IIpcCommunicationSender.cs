using System.Threading.Tasks;

namespace IpcWrapper.Library
{
    public interface IIpcCommunicationSender<in TValue>
    {
         Task WriteObject(TValue value);
    }
}