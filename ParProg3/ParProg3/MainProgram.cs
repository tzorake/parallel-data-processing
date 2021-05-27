using ParProg3.Solutions;
using System;
using System.Threading;

namespace ParProg3 {
    class MainProgram {
        static void Main(string[] args) {
            // threads without synchronization
            FirstSolution  firstSolution  = new FirstSolution(6, 3);
            // threads synchronization with 'lock'
            SecondSolution secondSolution = new SecondSolution(6, 3);
            // threads synchronization with 'AutoResetEvent'
            ThirdSolution  thirdSolution  = new ThirdSolution(6, 3);
            // threads synchronization with 'Semaphore'
            FourthSolution fourthSolution = new FourthSolution(6, 3);
            // threads synchronization with 'Interlocked'
            FifthSolution  fifthSolution  = new FifthSolution(6, 3);

            // firstSolution.Run();
            // secondSolution.Run();
            // thirdSolution.Run();
            // fourthSolution.Run();
            // fifthSolution.Run();

        }
    }
}
