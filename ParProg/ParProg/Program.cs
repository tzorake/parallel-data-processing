using System;
using System.Diagnostics;
using System.Threading;

namespace ParProg {
    class Program {
        static int from = -9, to = 9;
        static double multiplier = 2.0;

        static bool bLogging = true;

        static double Function(double x, double y) {
            return Math.Pow(x, y);
        }

        static void FirstProblem(VecNd vec) {
            int N = vec.Dimension;
            if (bLogging) Console.WriteLine(vec.ToString());
            for (int i = 0; i < vec.Dimension; i++) {
                vec.SetElement(i, Function(vec.Matrix[i], multiplier));
            }
            if (bLogging) Console.WriteLine(vec.ToString());
        }

        static void SecondProblem(VecNd vec, int M) {
            int N = vec.Dimension;
            if (bLogging) Console.WriteLine(vec.ToString());
            Thread[] threads = new Thread[M];
            for (int i = 0; i < M; i++) {
                threads[i] = new Thread(() => {
                    for (int j = i * (N / M); j < (i + 1 != M ? (i + 1) * (N / M) : N); j++)
                        vec.SetElement(j, Function(vec.Matrix[j], multiplier));
                });
                threads[i].Start();
                threads[i].Join();
            }
            if (bLogging) Console.WriteLine(vec.ToString());
        }

        static void ThirdProblem() { 
            int[] N = new int[] { 10, 100, 1000, 100000 };
            int[] M = new int[] { 2, 3, 4, 5, 10 };

            Stopwatch sw = new Stopwatch();

            for (int i = 0; i < M.Length; i++) {
                if (i == 0) Console.Write("\t");
                Console.Write("{0}\t", M[i]);
            }
            Console.Write("\n");
            for (int i = 0; i < N.Length; i++) {
                for (int j = 0; j < M.Length; j++) {
                    if (j == 0) Console.Write("{0}\t", N[i]);
                    sw.Reset();
                    sw.Start();
                    SecondProblem(new VecNd(N[i]).RandomInt(from, to), M[i]);
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    Console.Write("{0}\t", Math.Round(ts.TotalMilliseconds, 3));
                }
                Console.Write("\n");
            }
        }

        static void FourthProblem() { 
            int[] N = new int[] { 10, 100, 1000, 100000 };

            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < N.Length; i++) {
                if (i == 0) Console.Write("\t");
                Console.Write("{0}\t", N[i]);
            }
            Console.Write("\n");
            for (int i = 0; i < N.Length; i++) {
                if (i == 0) Console.Write("\t");
                sw.Reset();
                sw.Start();
                FirstProblem(new VecNd(N[i]).RandomInt(from, to));
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Console.Write("{0}\t", Math.Round(ts.TotalMilliseconds, 3));
            }
            Console.Write("\n");
        }

        static void FifthProblem(VecNd vec, int M) {
            int N = vec.Dimension;
            if (bLogging) Console.WriteLine(vec.ToString());
            Thread[] threads = new Thread[M];
            for (int i = 0; i < M; i++) {
                threads[i] = new Thread(() => {
                    for (int j = i; j < N ; j+=M)
                        vec.SetElement(j, Function(vec.Matrix[j], multiplier));
                });
                threads[i].Start();
                threads[i].Join();
            }
            if (bLogging) Console.WriteLine(vec.ToString());
        }

        static void FifthProblemWrapper() { 
            int[] N = new int[] { 10, 100, 1000, 100000 };
            int[] M = new int[] { 2, 3, 4, 5, 10 };

            Stopwatch sw = new Stopwatch();

            for (int i = 0; i < M.Length; i++) {
                if (i == 0) Console.Write("\t");
                Console.Write("{0}\t", M[i]);
            }
            Console.Write("\n");
            for (int i = 0; i < N.Length; i++) {
                for (int j = 0; j < M.Length; j++) {
                    if (j == 0) Console.Write("{0}\t", N[i]);
                    sw.Reset();
                    sw.Start();
                    FifthProblem(new VecNd(N[i]).RandomInt(from, to), M[i]);
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    Console.Write("{0}\t", Math.Round(ts.TotalMilliseconds, 3));
                }
                Console.Write("\n");
            }
        }

        static void Main(string[] args) {
            
            Console.WriteLine("First Problem:\n");
            int from = -9, to = 9;
            bLogging = true;
            FirstProblem(new VecNd(10).RandomInt(from, to));

            Console.WriteLine("\nSecond Problem:\n");
            from = -9; to = 9;
            bLogging = true;
            SecondProblem(new VecNd(10).RandomInt(from, to), 2);

            Console.WriteLine("\nThird Problem:\n");
            from = (int)-1e8; to = (int)1e8;
            bLogging = false;
            ThirdProblem();

            Console.WriteLine("\nFourth Problem:\n");
            bLogging = false;
            FourthProblem();

            Console.WriteLine("\nFifth Problem:\n");
            bLogging = false;
            FifthProblemWrapper();
        }
    }
}
