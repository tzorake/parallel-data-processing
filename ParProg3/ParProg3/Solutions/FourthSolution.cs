using System;
using System.Threading;

namespace ParProg3.Solutions {
    class FourthSolution {
        bool bEmpty, bFinished;
        string buffer;

        Thread[] writers;
        Thread[] readers;

        Semaphore writersSemaphore, readersSemaphore;
        public FourthSolution(int wCount, int rCount) {
            bEmpty = true;
            buffer = "";

            writersSemaphore = new Semaphore(1, 1);
            readersSemaphore = new Semaphore(1, 1);

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
            writersSemaphore.WaitOne();
            while (!bEmpty) ;
            Console.WriteLine($"[0] writer{id} began his work.");
            buffer = new string(Guid.NewGuid().ToString());
            bEmpty = false;
            Console.WriteLine($"[1] writer{id} finished his work.");
            writersSemaphore.Release();
        }

        void Reader(object o) {
            int id = (int)o;
            Console.WriteLine($"[0] reader{id} began his work.");
            while (!bFinished) {
                readersSemaphore.WaitOne();
                if (!bEmpty) {
                    Console.WriteLine($"[*] reader{id} got a message: {buffer}");
                    bEmpty = true;
                }
                readersSemaphore.Release();
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
