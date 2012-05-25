using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class TaskOperations
    {
        public void UseTask()
        {
            var t = Task.Delay(2000);
            t.ContinueWith(_ => Console.WriteLine("Done executing task"));
        }

        public void UseTaskFactory()
        {
            var t = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("Executing task");
            });

        }

        public void TaskWithResult()
        {
            Task<string> t = new WebClient().DownloadStringTaskAsync("http://squeed.com");
            t.ContinueWith(_ => Console.WriteLine("Task result: " + t.Result));

        }

        public void TaskWithException()
        {
            Task<string> t = new WebClient().DownloadStringTaskAsync("http://squeed.com.invalidAddress");
            t.ContinueWith(_ =>
            {
                //Console.WriteLine(t.Result);
                if (t.IsFaulted)
                {
                    Console.WriteLine("Task exception: " + t.Exception.ToString());
                }
            });
        }
    }
}