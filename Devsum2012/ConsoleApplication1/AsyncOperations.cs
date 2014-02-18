using System;
using System.Threading;

namespace ConsoleApplication1
{
    public class AsyncOperations
    {
        public void UseTimer()
        {
            new Timer(_ => Console.WriteLine("Run work")).Change(2000, -1);
        }

        public void UseThread()
        {
            new Thread(
                () =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine("Executing work in thread ");
                }).Start();
        }

        public void UseThreadLoop()
        {
            for (var i = 0; i < 10000; i++)
            {
                int i1 = i;
                new Thread(
                    () =>
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine("Executing work in thread " + i1);
                    }).Start();
            }
        }

        public void TimerLoop()
        {
            for (int i = 0; i < 10000; i++)
            {
                int i1 = i;
                new Timer(_ => Console.WriteLine("Executing work with timer (async): " + i1)).Change(2000, -1);
            }
        }
    }
}