using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    //Simple sum of squares
    public class Comparison
    {
        public Comparison()
        {
            Console.WriteLine("Sum of squares of 1-100: {0}", SumOfSquares(10));
        }

        public int Square(int num)
        {
            return num * num;
        }

        public int SumOfSquares(int number)
        {
            var sum = 0;
            foreach (var num in Enumerable.Range(1, number))
            {
                sum += Square(num);
            }
            return sum;
        }





        //public int SumOfLinq(int number)
        //{
        //    return Enumerable.Range(1, number)
        //           .Sum(num => Square(num));
        //}
    }


//QuickSort
//If the list is empty, there is nothing to do.
//Otherwise: 
//  1. Take the first element of the list
//  2. Find all elements in the rest of the list that 
//      are less than the first element, and sort them. 
//  3. Find all elements in the rest of the list that 
//      are >= than the first element, and sort them
//  4. Combine the three parts together to get the final result: 
//      (sorted smaller elements + firstElement + 
//       sorted larger elements)
    public class QuickSortHelper
    {
        public static List<T> QuickSort<T>(List<T> values)
           where T : IComparable
        {
            if (values.Count == 0)
            {
                return new List<T>();
            }

            T firstElement = values[0];

            var smallerElements = new List<T>();
            var largerElements = new List<T>();
            for (int i = 1; i < values.Count; i++)  // i starts at 1
            {                                       // not 0!
                var elem = values[i];
                if (elem.CompareTo(firstElement) < 0)
                {
                    smallerElements.Add(elem);
                }
                else
                {
                    largerElements.Add(elem);
                }
            }

            var result = new List<T>();
            result.AddRange(QuickSort(smallerElements.ToList()));
            result.Add(firstElement);
            result.AddRange(QuickSort(largerElements.ToList()));
            return result;
        }
    }


    public static class QuickSortExtension
    {
        public static IEnumerable<T> QuickSort<T>(
            this IEnumerable<T> values) where T : IComparable
        {
            if (values == null || !values.Any())
            {
                return new List<T>();
            }

            var firstElement = values.First();
            var rest = values.Skip(1);

            var smallerElements = rest
                    .Where(i => i.CompareTo(firstElement) < 0)
                    .QuickSort();

            var largerElements = rest
                    .Where(i => i.CompareTo(firstElement) >= 0)
                    .QuickSort();

            return smallerElements
                .Concat(new List<T> { firstElement })
                .Concat(largerElements);
        }
    }
}



