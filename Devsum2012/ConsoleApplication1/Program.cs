using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //new AsyncOperations().UseTimer();
            //new AsyncOperations().UseThread();
            //new AsyncOperations().UseThreadLoop();
            //new AsyncOperations().TimerLoop();

            //new TaskOperations().UseTask();
            //new TaskOperations().UseTaskFactory();
            //new TaskOperations().TaskWithResult();
            //new TaskOperations().TaskWithException();

            //new AwaitOperations().UseAwait();
            //new AwaitOperations().UseAwaitWithException();
            //new AwaitOperations().AwaitLoop();
            //new AwaitOperations().AsyncFunction();
            //new AwaitOperations().UnOrderedExecution();
            //new AwaitOperations().InfiniteLoop();
            //new AwaitOperations().Cancellation();
            //new AwaitOperations().ProgressReport();
            //new AwaitOperations().RunInParallel();

            //new RxOperations().AwaitObservable();
            //new RxOperations().ToAsync();
            //new RxOperations().LoopAsync();
            new RxOperations().CombineSources();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
