using System;
using System.Threading;

namespace ParProg3.Solutions {
    class SecondSolution {
        bool bEmpty, bFinished;
        int messageCounter;

        string buffer;
        Thread[] writers;
        Thread[] readers;


        object lockingReader;
        object lockingWriter;

        // where wCount is writers count, rCount is readers count
        public SecondSolution(int wCount, int rCount) {
            bEmpty = true;
            messageCounter = 0;

            buffer = "";

            lockingReader = new object();
            lockingWriter = new object();

            writers = new Thread[wCount];
            for (int i = 0; i < wCount; i++) {
                writers[i] = new Thread(Writer);
                // writers[i].Start(i);
            }

            readers = new Thread[rCount];
            for (int i = 0; i < rCount; i++) {
                readers[i] = new Thread(Reader);
                // readers[i].Start(i);
            }
        }

        void Writer(object o) {
            int id = (int)o;
            Console.WriteLine($"[0] writer{id} began his work.");
            while (messageCounter != writers.Length) {
                if (bEmpty) { 
                    lock (lockingWriter) {
                        if (bEmpty) {
                            buffer = new string(Guid.NewGuid().ToString());
                            bEmpty = false;
                            messageCounter++;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine($"[1] writer{id} finished his work.");
        }

        void Reader(object o) {
            int id = (int)o;
            Console.WriteLine($"[0] reader{id} began his work.");
            while (!bFinished) {
                if (!bEmpty) { 
                    lock (lockingReader) {
                        if(!bEmpty) {
                            Console.WriteLine($"[*] reader{id} got a message: {buffer}");
                            bEmpty = true;
                        }
                    }
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
