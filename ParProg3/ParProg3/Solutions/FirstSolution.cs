using System;
using System.Threading;

namespace ParProg3.Solutions {
    class FirstSolution {
        bool bEmpty, bFinished;
        int messageCounter;

        string buffer;
        Thread[] writers;
        Thread[] readers;

        // where wCount is writers count, rCount is readers count
        public FirstSolution(int wCount, int rCount) {
            buffer = "";

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
            Console.WriteLine($"[0] writer{id} began his work.");
            while (messageCounter != writers.Length) { 
                if (bEmpty) {
                    buffer = new string(Guid.NewGuid().ToString());
                    bEmpty = false;
                    messageCounter++;
                    break;
                }
            }
            Console.WriteLine($"[1] writer{id} finished his work.");
        }

        void Reader(object o) {
            int id = (int)o;
            Console.WriteLine($"[0] reader{id} began his work.");
            while (!bFinished) { 
                if (!bEmpty) {
                    Console.WriteLine($"[*] reader{id} got a message: {buffer}");
                    bEmpty = true;
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
                int counter = 0;
                foreach (var w in writers) if (!w.IsAlive) counter++;
                if (writers.Length == counter) break;
            }

            bFinished = true;

            while (true) {
                int counter = 0;
                foreach (var r in readers) if (!r.IsAlive) counter++;
                if (readers.Length == counter) break;
            }

            Console.WriteLine("Done!");
        }
    }
}
