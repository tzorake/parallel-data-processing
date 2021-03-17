using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParProg2
{
    class Test {
        static List<int> simpleSieve(int from, int to) {
            if (from <= 1) from = 2;

            List<int> result = new List<int>();

            bool[] marks = new bool[to + 1];
            for (int i = 0; i < marks.Length; i++) marks[i] = true;

            for (int i = 2; i < to; i++) {
                if (marks[i] == true) {
                    for (int j = 2*i; j < to + 1; j += i) marks[j] = false;
                }
            }
            for (int i = from; i < to + 1; i++) {
                if (marks[i]) result.Add(i);
            }

            return result;
        }

        static List<int> modifiedSieve(int from, int to) {
            if (from <= 1) from = 2;

            int bound = (int)Math.Sqrt(to);

            List<int> primes = simpleSieve(2, bound);
            List<int> result = new List<int>();

            bool[] marks = new bool[to - from + 1];
            for (int i = 0; i < marks.Length; i++) marks[i] = true;

            for (int i = 0; i < primes.Count; i++) {
                int low = 0;
                while (low < from) low += primes[i];
                for (int j = low; j < to + 1; j += primes[i]) {
                    marks[j - from] = false;
                }
            }

            if (from < bound) result.AddRange(primes);

            for (int i = from; i < to + 1; i++) {
                if (marks[i - from]) result.Add(i);
            }

            return result;
        }

        static List<int> SieveV1(int from, int to, int M) {
            if (from <= 1) from = 2;

            int bound = (int)Math.Sqrt(to);

            List<int> primes = simpleSieve(2, bound);
            // List<int> preproc = new List<int>();
            List<int> result = new List<int>();

            bool[] marks = new bool[to + 1];

            int step = (to - from) / M;

            Thread[] threads = new Thread[M];
            for (int i = 0; i < M; i++) {
                threads[i] = new Thread(() => {
                    result.AddRange(simpleSieve(from + i * step, i + 1 == M ? to : from + (i + 1) * step - 1));
                });
                threads[i].Start();
                threads[i].Join();
            }

            return result;
        }

        static List<int> SieveV2(int from, int to, int M) {
            if (from <= 1) from = 2;

            int bound = (int)Math.Sqrt(to);

            List<int> primes = new List<int>(); // simpleSieve(2, bound);
            List<int> result = new List<int>();

            bool[] marks = new bool[to - from + 1];
            for (int i = 0; i < marks.Length; i++) marks[i] = true;

            int step = (bound - 2) / 2;

            Thread[] threads = new Thread[M];
            for (int i = 0; i < M; i++) {
                threads[i] = new Thread(() => {
                    List<int> partOfPrimes = simpleSieve(2 + i * step, i + 1 == M ? bound : 2 + (i + 1) * step - 1);
                    for (int i = 0; i < partOfPrimes.Count; i++) { 
                        int low = 0;
                        while (low < from) low += partOfPrimes[i];
                        for (int j = low; j < to + 1; j += partOfPrimes[i]) {
                            marks[j - from] = false;
                        }
                    }
                    primes.AddRange(partOfPrimes);
                });
                threads[i].Start();
                threads[i].Join();
            }

            if (from < bound) result.AddRange(primes);

            for (int i = from; i < to + 1; i++) {
                if (marks[i - from]) result.Add(i);
            }

            return result;
        }

        static List<int> SieveV3(int from, int to) {
            if (from <= 1) from = 2;

            int bound = (int)Math.Sqrt(to);

            List<int> primes = simpleSieve(2, bound);
            List<int> result = new List<int>();

            bool[] marks = new bool[to + 1];
            for (int i = 0; i < marks.Length; i++) marks[i] = true;

            ManualResetEvent[] manualEvents = new ManualResetEvent[primes.Count];
            for (int i = 0; i < manualEvents.Length; i++)
                manualEvents[i] = new ManualResetEvent(false);

            for (int i = 0; i < primes.Count; i++) {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Handler), new object[] { primes[i], to, manualEvents[i], marks });
            }
            WaitHandle.WaitAll(manualEvents);

            for (int i = from; i < to + 1; i++) {
                if (marks[i]) result.Add(i);
            }

            return result;
        }

        static void Handler(object o) {
            int prime = (int)((object[])o)[0];
            int to = (int)((object[])o)[1];
            ManualResetEvent ev = (ManualResetEvent)(((object[])o)[2]);

            for (int j = 2 * prime; j < to + 1; j += prime) {
                ((bool[])((object[])o)[3])[j] = false;
            }
            ev.Set();
        }

        static List<int> SieveV4(int from, int to) {
            if (from <= 1) from = 2;

            IEnumerable<int> primes = Enumerable.Range(from, to - 1)
                                                .Where(number => Enumerable.Range(2, (int)Math.Sqrt(number) - 1)
                                                .All(divisor => number % divisor != 0));

            return primes.ToList();
        }

        static List<int> SieveV5(int from, int to) {
            if (from <= 1) from = 2;

            int bound = (int)Math.Sqrt(to);

            List<int> primes = simpleSieve(2, bound);
            List<int> result = new List<int>();

            bool[] marks = new bool[to + 1];
            for (int i = 0; i < marks.Length; i++) marks[i] = true;

            Parallel.For(0, primes.Count, (i, state) => {
                for (int j = 2 * primes[i]; j < to + 1; j += primes[i]) marks[j] = false;
            });

            for (int i = from; i < to + 1; i++) {
                if (marks[i]) result.Add(i);
            }

            return result;
        }

        static List<int> SieveV6(int from, int to) {
            if (from <= 1) from = 2;

            int bound = (int)Math.Sqrt(to);

            List<int> primes = simpleSieve(2, bound);
            List<int> result = new List<int>();

            bool[] marks = new bool[to + 1];
            for (int i = 0; i < marks.Length; i++) marks[i] = true;

            Action<object> action = (object o) => {
                for (int j = 2 * primes[(int)o]; j < to + 1; j += primes[(int)o]) marks[j] = false;
            };

            Task[] tasks = new Task[primes.Count];
            for (int i = 0; i < tasks.Length; i++) {
                tasks[i] = new Task(action, i);
                tasks[i].Start();
            }

            for (int i = from; i < to + 1; i++) {
                if (marks[i]) result.Add(i);
            }

            return result;
        }

        static void Show(List<int> list) {
            for (int i = 0; i < list.Count; i++)
                Console.Write("{0}\t", list[i]);
            Console.Write("\n");
        }

        static void ShowTime(string item, int from, int to, int M) {
            List<int> result;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            switch (item) {
                case "simple":
                    result = simpleSieve(from, to);
                    break;
                case "modified":
                    result = modifiedSieve(from, to);
                    break;
                case "v1":
                    result = SieveV1(from, to, M);
                    break;
                case "v2":
                    result = SieveV2(from, to, M);
                    break;
                case "v3":
                    result = SieveV3(from, to);
                    break;
                case "v4":
                    result = SieveV4(from, to);
                    break;
                case "v5":
                    result = SieveV5(from, to);
                    break;
                case "v6":
                    result = SieveV6(from, to);
                    break;
                default:
                    break;
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Console.Write("Time: {0}\n\n", Math.Round(ts.TotalMilliseconds, 3));
        }

        public static void Main() {
            int from = 0, to = 91000, M = 2;
            bool show = false;

            Console.Write("Simple Sieve: \n");
            List<int> result1 = simpleSieve(from, to);
            if (show) Show(result1);
            ShowTime("simple", from, to, 0);

            Console.Write("Modified Sieve: \n");
            List<int> result2 = modifiedSieve(from, to);
            if (show) Show(result2);
            ShowTime("modified", from, to, 0);

            Console.Write("Sieve v.1: \n");
            List<int> result3 = SieveV1(from, to, M);
            if (show) Show(result3);
            ShowTime("v1", from, to, M);

            Console.Write("Sieve v.2: \n");
            List<int> result4 = SieveV2(from, to, M);
            if (show) Show(result4);
            ShowTime("v2", from, to, M);

            Console.Write("Sieve v.3 (via ThreadPool): \n");
            List<int> result5 = SieveV3(from, to);
            if (show) Show(result5);
            ShowTime("v3", from, to, 0);

            Console.Write("Sieve v.4 (via LINQ): \n");
            List<int> result6 = SieveV4(from, to);
            if (show) Show(result6);
            ShowTime("v4", from, to, 0);

            Console.Write("Sieve v.5 (via Parallel.For): \n");
            List<int> result7 = SieveV5(from, to);
            if (show) Show(result7);
            ShowTime("v5", from, to, 0);

            Console.Write("Sieve v.6 (via Task): \n");
            List<int> result8 = SieveV6(from, to);
            if (show) Show(result6);
            ShowTime("v6", from, to, 0);
        }
    }
}
