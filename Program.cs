using System;
using System.Collections.Generic;

namespace MIP_LAB2_exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. lépés: bekérjük, hány szálat indítsunk (1 és 10 között)
            int numberOfThreads = ReadNumberOfThreads();

            // 2. lépés: létrehozzuk a szálakat és eltároljuk őket egy listában
            List<WorkerThread> workerThreads = new List<WorkerThread>();

            for (int index = 0; index < numberOfThreads; index++)
            {
                WorkerThread newWorker = new WorkerThread();
                workerThreads.Add(newWorker);
            }

            // 3. lépés: elindítjuk az összes szálat
            foreach (WorkerThread worker in workerThreads)
            {
                worker.Start();
            }

            // 4. lépés (opcionális): a felhasználó leállíthat egy szálat az ID-ja alapján
            AskUserWhichThreadToCancel(workerThreads);

            // 5. lépés: megvárjuk, hogy minden szál befejezze a futását
            foreach (WorkerThread worker in workerThreads)
            {
                worker.WaitUntilFinished();
            }

            Console.WriteLine("Minden szál befejezte a futást. Nyomj meg egy gombot a kilépéshez.");
            Console.ReadKey();
        }

        // Addig kérdezi, amíg nem kap 1 és 10 közötti egész számot
        private static int ReadNumberOfThreads()
        {
            int numberOfThreads = 0;
            bool inputIsValid = false;

            while (inputIsValid == false)
            {
                Console.Write("Hány szálat indítsak (1-10)? ");
                string userInput = Console.ReadLine();

                bool isNumber = int.TryParse(userInput, out numberOfThreads);

                if (isNumber && numberOfThreads >= 1 && numberOfThreads <= 10)
                {
                    inputIsValid = true;
                }
                else
                {
                    Console.WriteLine("Hibás érték! 1 és 10 közötti egész számot adj meg.");
                }
            }

            return numberOfThreads;
        }

        // Addig kér ID-t, amíg a felhasználó üres sort nem ír (Enter),
        // vagy amíg az összes szál le nem állt.
        private static void AskUserWhichThreadToCancel(List<WorkerThread> workerThreads)
        {
            bool userWantsToContinue = true;

            while (userWantsToContinue && AnyThreadIsRunning(workerThreads))
            {
                Console.WriteLine("Írd be a leállítandó szál ID-ját (Enter = csak várakozás):");
                string userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                {
                    userWantsToContinue = false;
                }
                else
                {
                    int threadIdToCancel;
                    bool isNumber = int.TryParse(userInput, out threadIdToCancel);

                    if (isNumber)
                    {
                        CancelThreadById(workerThreads, threadIdToCancel);
                    }
                    else
                    {
                        Console.WriteLine("Az ID egész szám kell, hogy legyen.");
                    }
                }
            }
        }

        // Megkeresi a megadott ID-jú szálat, és ha fut, megszakítja
        private static void CancelThreadById(List<WorkerThread> workerThreads, int threadIdToCancel)
        {
            bool threadFound = false;

            foreach (WorkerThread worker in workerThreads)
            {
                if (worker.ThreadId == threadIdToCancel)
                {
                    threadFound = true;

                    if (worker.IsRunning)
                    {
                        worker.Cancel();
                    }
                    else
                    {
                        Console.WriteLine("A(z) " + threadIdToCancel + " ID-jú szál már leállt.");
                    }
                }
            }

            if (threadFound == false)
            {
                Console.WriteLine("Nincs ilyen ID-jú szál: " + threadIdToCancel);
            }
        }

        private static bool AnyThreadIsRunning(List<WorkerThread> workerThreads)
        {
            foreach (WorkerThread worker in workerThreads)
            {
                if (worker.IsRunning)
                {
                    return true;
                }
            }

            return false;
        }
    }
}