using System;
using System.Threading.Tasks;

namespace IpcWrapper.App
{
    public class Program
    {
        public static async Task Main(string[] args) => await Task.Run(() => Console.WriteLine("Hello World!"));
    }
}
