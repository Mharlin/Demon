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
                new Thread(
                    () =>
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine("Executing work in thread " + i);
                    }).Start();
            }

        }

        public void TimerLoop()
        {
            for (int i = 0; i < 10000; i++)
            {
                new Timer(_ => Console.WriteLine("Executing work with timer (async)")).Change(2000, -1);
            }
        }
    }
}