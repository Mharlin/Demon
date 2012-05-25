using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class SimulatedWebClient
    {
        public async Task<string> DownloadStringTaskAsync(string uri)
        {
            await Task.Delay(new Random().Next(2000));
            return "Request to uri: " + uri + Environment.NewLine + "Request was successfull";
        }

        public string DownloadString(string uri)
        {
            Thread.Sleep(new Random().Next(2000));
            return "Request to uri: " + uri + Environment.NewLine + "Request was successfull";            
        }
    }
}