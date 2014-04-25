using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParallelismDemo
{
    public static class MyUtils
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action) {
            foreach (var item in list) action(item);
        }
    }
    class Program
    {
        private static int numberCount = 400000;
        static void Main(string[] args)
        {
            sequential();
            ParallelWithGlobal();
            ParallelWithLocal();
            Console.ReadLine();

        }

        private static void sequential() {
            var start = 100000;
            var sw = new Stopwatch();
            var primeCount = 0;
            sw.Start();

            Enumerable.Range(start, numberCount).ForEach((n) =>
            {
                if (IsPrime(n)) ++primeCount;
            });
            sw.Stop();

            
            Console.WriteLine("[Sequential] -> {0} prime numbers are found in {1} millisecods", primeCount, sw.ElapsedMilliseconds);
            
        }

        private static void ParallelWithGlobal()
        {
            var start = 100000;
            var sw = new Stopwatch();
            var primeCount = 0;
            sw.Start();

            var o = new Object();
            Parallel.For(100000, start + numberCount + 1, (n) =>
            {

                if (IsPrime(n))
                {
                    lock (o)
                    {
                        ++primeCount;
                    }

                }
            });

            sw.Stop();

            Console.WriteLine("[ParallelWithGlobal] -> {0} prime numbers are found in {1} millisecods", primeCount, sw.ElapsedMilliseconds);

        }

        private static void ParallelWithLocal()
        {
            var start = 100000;
            var sw = new Stopwatch();
            var primeCount = 0;
            sw.Start();

            Parallel.For<int>(100000, start + numberCount + 1, () => 0, (n, op, intermediateCounter) =>
            {
                if (IsPrime(n)) return ++intermediateCounter;
                return intermediateCounter;
            }, (intermediateCounter) => primeCount += intermediateCounter);

            sw.Stop();

            Console.WriteLine("[ParallelWithLocal] -> {0} prime numbers are found in {1} millisecods", primeCount, sw.ElapsedMilliseconds);

        }

        private static bool IsPrime(int number)
        {
            for (var i = 2; i <= number / 2; i++)
                if (number % i == 0) return false;
            return true;
        }
    }
}
