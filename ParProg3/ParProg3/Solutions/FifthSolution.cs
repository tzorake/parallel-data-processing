using System;
using System.Threading;

namespace ParProg3.Solutions {
    class FifthSolution {
        bool bEmpty, bFinished;
        string buffer;

        Thread[] writers;
        Thread[] readers;

        int writersVar, readersVar;
        public FifthSolution(int wCount, int rCount) {
            bEmpty = true;
            buffer = "";

            writersVar = 0;
            readersVar = 0;

            writers = new Thread[wCount];
            for (int i = 0; i < wCount; i++) {
                writers[i] = new Thread(Writer);
            }

            readers = new Thread[rCount];
            for (int i = 0; i < rCount; i++) {
                readers[i] = new Thread(Reader);
            }
        }

        void Writer(object o) {
            int id = (int)o;
            while (true) {
                if (0 == Interlocked.Exchange(ref writersVar, 1)) {
                    while (!bEmpty) ;
                    Console.WriteLine($"[0] writer{id} began his work.");
                    buffer = new string(Guid.NewGuid().ToString());
                    bEmpty = false;
                    Console.WriteLine($"[1] writer{id} finished his work.");
                    Interlocked.Exchange(ref writersVar, 0);
                    break;
                }
            }
        }

        void Reader(object o) {
            int id = (int)o;
            Console.WriteLine($"[0] reader{id} began his work.");
            while (true) {
                if (bFinished) break;
                if (0 == Interlocked.Exchange(ref readersVar, 1)) {
                    while (bEmpty) ;
                    Console.WriteLine($"[*] reader{id} got a message: {buffer}");
                    bEmpty = true;
                    Interlocked.Exchange(ref readersVar, 0);
                }     
            }
            Console.WriteLine($"[1] reader{id} finished his work.");
        }

        public void Run() {
            for (int i = 0; i < writers.Length; i++) {
                writers[i].Start(i);
            }

            for (int i = 0; i < readers.Length; i++) {
                readers[i].Start(i);
            }

            while (true) {
                int i = 0;
                foreach (var w in writers) if (!w.IsAlive) i++;
                if (writers.Length == i) {
                    bFinished = true;
                    break;
                }
            }

            while (true) {
                int i = 0;
                foreach (var r in readers) if (!r.IsAlive) i++;
                if (readers.Length == i) break;
            }

            Console.WriteLine("Done!");
        }
    }
    
}



/*

Interlocked.Exchange(ref usingResource, 1)

Thread.Sleep(500);

Interlocked.Exchange(ref usingResource, 0);

*/