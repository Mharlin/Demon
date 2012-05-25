using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class AwaitOperations
    {
        public async void UseAwait()
        {
            string result = await new WebClient().DownloadStringTaskAsync("http://squeed.com");
            Console.WriteLine("Task result: " + result);
        }

        public async void UseAwaitWithException()
        {
            try
            {
                string result = await new WebClient().DownloadStringTaskAsync("http://squeed.com.invalidAddress");
                Console.WriteLine("Task result: " + result);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught Exception: " + ex.ToString());
            }
        }

        public async void AwaitLoop()
        {
            var uris = new[]
            {
                "http://squeed.com",
                "http://www.squeed.com/innovation",
                "http://www.squeed.com/om-oss",
                "http://www.squeed.com/blog/",
                "http://www.squeed.com/jobb"
            };

            foreach (var uri in uris)
            {
                var result = await new WebClient().DownloadStringTaskAsync(uri);
                Console.WriteLine("Size of download: " + result.Length + Environment.NewLine + "URI: " + uri + Environment.NewLine);
            }
        }

        public async void AsyncFunction()
        {
            var result =  await AwaitLoopReturningTask();
            Console.WriteLine(result);
        }

        private async Task<string> AwaitLoopReturningTask()
        {
            var uris = new[]
            {
                "http://squeed.com",
                "http://www.squeed.com/innovation",
                "http://www.squeed.com/om-oss",
                "http://www.squeed.com/blog/",
                "http://www.squeed.com/jobb"
            };

            foreach (var uri in uris)
            {
                var result = await new WebClient().DownloadStringTaskAsync(uri);
                Console.WriteLine("Size of download: " + result.Length + Environment.NewLine + "URI: " + uri + Environment.NewLine);
            }

            return "All operations completed successfully";
        }

        public void UnOrderedExecution()
        {
            for (int i = 0; i < 20; i++)
            {
                PrintResult(i);
            }
        }

        private async Task PrintResult(int number)
        {
            await Task.Delay(1000);
            Console.WriteLine("Iteration: " + number);
        }

        public void InfiniteLoop()
        {
            FirstLoop();

            SecondLoop();
        }

        private async void FirstLoop()
        {
            while (true)
            {
                Func<Task> task =
                    async () =>
                    {
                        await Task.Delay(700);
                        Console.WriteLine("----------------");
                    };

                await task();
            }
        }

        private async void SecondLoop()
        {
            while (true)
            {
                Func<Task> task =
                    async () =>
                    {
                        await Task.Delay(300);
                        Console.WriteLine("****************");
                    };

                await task();
            }
        }

        public void Cancellation()
        {
            var cancellationToken = new CancellationTokenSource();

            RunLoopWithCancellation(cancellationToken.Token);

            Console.WriteLine("Press any key to cancel operation");
            Console.ReadKey();
            cancellationToken.Cancel();

        }

        private async void RunLoopWithCancellation(CancellationToken token)
        {
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(200);
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Operation cancelled");
                    return;
                }

                Console.WriteLine("Iteration: " + i);
            }
        }

        public void ProgressReport()
        {
            var progressReporting = new Progress<int>(i => Console.WriteLine("Progress: " + i + "%"));

            RunLoopWithProgressReport(progressReporting);
        }

        private async void RunLoopWithProgressReport(IProgress<int> progress)
        {
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(200);
                progress.Report(i);
            }
        }

        public async void RunInParallel()
        {
            Func<Task> func1 = async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("First Task executed");
            };

            Func<Task> func2 = async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Second Task executed");
            };

            Func<Task> func3 = async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Third Task executed");
            };

            await func1();
            await func2();
            await func3();

            //Task.WhenAll(func1(), func2(), func3());
        }
    }
}