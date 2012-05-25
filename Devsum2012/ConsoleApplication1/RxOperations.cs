using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class RxOperations
    {
        public async void AwaitObservable()
        {
            var result = await Observable.Return("Executing observable operation").Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine(result);
        }

        public async void ToAsync()
        {
            await Observable.ToAsync(() => LegacyLoop())();
        }

        private void LegacyLoop()
        {
            Thread.Sleep(1000);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Running loop, iteration: " + i);
            }
        }

        public async Task LoopAsync()
        {
            var results = LoopWithResults().ToObservable();

            //results.Subscribe(async i =>
            //{
            //    await Task.Delay(1000);
            //    Console.WriteLine("First loop iteration: " + i);
            //});

            //await results
            //   .Do(async i =>
            //   {
            //       await Task.Delay(2000);
            //       Console.WriteLine("Iteration " + i);
            //   });

            await results
                .Where(i => i > 4)
                .Select(i => i * 2)
                .ForEachAsync(async i =>
                {
                    await Task.Delay(2000);
                    Console.WriteLine("Transformed result: " + i);
                });

            
        }

        public void CombineSources()
        {
            var range = Observable.Range(1, 10).Delay(TimeSpan.FromMilliseconds(500));

            var results = from r in range
                          from result in LoopWithResults().ToObservable()
                          select r * result;

            results.Subscribe(r => Console.WriteLine("Results from combine operation:" + r));


        }

        private IEnumerable<int> LoopWithResults()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return i;
            }
        }
        


    }
}