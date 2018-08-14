using System;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using IpcWrapper.Library;

namespace IpcWrapper.NamedPipes
{
    public class NamedPipeObjectWriter<TValue> : IIpcCommunicationSender<TValue>
    {
        private readonly PipeStream baseStream;

        public NamedPipeObjectWriter(PipeStream baseStream)
        {
            this.baseStream = baseStream;
        }

        public async Task WriteObject(TValue value)
        {
            var data = Serialize(value);
            await WriteLengthAsync(data.Length);
            await WriteObjectAsync(data);
            await baseStream.FlushAsync();
        }

        private async Task WriteLengthAsync(int len)
        {
            var buffer = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(len));
            await baseStream.WriteAsync(buffer, 0, buffer.Length);
        }
        private async Task WriteObjectAsync(byte[] objBytes)
        {
            await baseStream.WriteAsync(objBytes, 0, objBytes.Length);
        }

        private byte[] Serialize(TValue obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);

                return memoryStream.ToArray();
            }
        }
    }
}