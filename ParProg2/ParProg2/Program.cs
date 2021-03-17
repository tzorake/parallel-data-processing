//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;

///*
//algorithm Sieve of Eratosthenes is
//    input: an integer n > 1.
//    output: all prime numbers from 2 through n.

//    let A be an array of Boolean values, indexed by integers 2 to n,
//    initially all set to true.

//    for i = 2, 3, 4, ..., not exceeding √n do
//        if A[i] is true
//            for j = i^2, i^2+i, i^2+2i, i^2+3i, ..., not exceeding n do
//                A[j] := false

//    return all i such that A[i] is true.
//*/

//namespace ParProg2 {
//    class Program {
//        static List<int> simpleSieve(int from, int to) {
//            if (from <= 1) from = 2;
//            List<int> result = new List<int>();
//            bool[] marks = new bool[to + 1];

//            for (int i = 0; i < to + 1; i++) marks[i] = true;

//            for (int i = 2; i < to; i++) {
//                if (marks[i] == true) {
//                    for (int j = 2*i; j < to + 1; j += i)
//                        marks[j] = false;
//                }
//            }
//            for (int i = from; i < to + 1; i++) {
//                if (marks[i])
//                    result.Add(i);
//            }

//            return result;
//        }

//        static List<int> modifiedSieve(int from, int to) {
//            List<int> result = new List<int>();

//            int bound = (int)(Math.Floor(Math.Sqrt(to)) + 1);
//            List<int> primes = simpleSieve(2, bound);
            
//            bool[] marks = new bool[to - bound + 1];

//            for (int i = 0; i < marks.Length; i++) marks[i] = true;

//            for (int i = 0; i < primes.Count; i++) {
//                int lowBound = 0;
//                while (lowBound < bound) lowBound += primes[i];

//                for (int j = lowBound + primes[i]; j < to + 1; j += primes[i])
//                    marks[j - bound] = false;
//            }

//            if (from < bound) result.AddRange(primes);

//            for (int i = bound; i < to + 1; i++) {
//                if(marks[i - bound] == true && i > from) 
//                    result.Add(i);
//            }
//            return result;
//        }

//        static List<int> segmentedSieve(int from, int to, int M) {
//            if (from <= 1) from = 2;
//            List<int> result = new List<int>();

//            int step = (to - from) / M;

//            Thread[] threads = new Thread[M];
//            for (int i = 0; i < M; i++) {
//                threads[i] = new Thread(() => {
//                    result.AddRange(modifiedSieve(from + i *step, (i + 1 == M ? to : from + (i + 1) * step - 1)));
//                });
//                threads[i].Start();
//                threads[i].Join();
//            }

//            return result;
//        }

//        static List<int> modifiedSegmentedSieve(int from, int to, int M) {
//            if (from <= 1) from = 2;

//            List<int> result = new List<int>();
//            List<int> primesR = new List<int>();
//            int bound = (int)(Math.Floor(Math.Sqrt(to)) + 1);
//            int step = (bound - 2) / M;

//            bool[] marks = new bool[to - bound + 1];
//            for (int i = 0; i < marks.Length; i++) marks[i] = true;


//            Thread[] threads = new Thread[M];
//            for (int i = 0; i < M; i++) {
//                threads[i] = new Thread(() => {
//                    List<int> primes;

//                    primes = ((i + 1 == M) ? simpleSieve(2 + i * step, bound) : simpleSieve(2 + i * step, 2 + (i + 1) * step - 1));
//                    primesR.AddRange(primes);

//                    for (int i = 0; i < primes.Count; i++) {
//                        int lowBound = 0;
//                        while (lowBound < bound) lowBound += primes[i];

//                        for (int j = lowBound + primes[i]; j < to + 1; j += primes[i])
//                            marks[j - bound] = false;
//                    }
//                });
//                threads[i].Start();
//                threads[i].Join();
//            }

//            if (from < bound) result.AddRange(primesR);

//            for (int j = bound; j < to + 1; j++) {
//                if (marks[j - bound] == true && j > from)
//                    result.Add(j);
//            }
//            return result;
//        }

//        static void Main() {
//            int from = 0, to = 100;

//            List<int> result1 = simpleSieve(from, to);
//            for (int i = 0; i < result1.Count; i++)
//                Console.Write("{0}\t", result1[i]);

//            Console.Write("\n");

//            List<int> result2 = modifiedSieve(from, to);
//            for (int i = 0; i < result2.Count; i++)
//                Console.Write("{0}\t", result2[i]);

//            Console.Write("\n");

//            List<int> result3 = segmentedSieve(from, to, 2);
//            for (int i = 0; i < result3.Count; i++)
//                Console.Write("{0}\t", result3[i]);

//            Console.Write("\n");

//            List<int> result4 = modifiedSegmentedSieve(from, to, 2);
//            for (int i = 0; i < result4.Count; i++)
//                Console.Write("{0}\t", result4[i]);

//        }
//    }
//}