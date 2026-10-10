using System;
using System.Threading;

namespace MIP_LAB2_exercises
{
    // Egyetlen szálat kezel: elindítja, 10 másodpercig futtatja,
    // és kérésre idő előtt megszakítja.
    public class WorkerThread
    {
        private const int RunTimeInMilliseconds = 10000;   // 10 másodperc
        private const int SleepStepInMilliseconds = 100;   // ennyi időnként nézzük meg, kérték-e a leállítást

        private Thread thread;
        private volatile bool cancelRequested;

        public WorkerThread()
        {
            // A szálat itt hozzuk létre, de még nem indítjuk el.
            // Az ID már a létrehozás után elérhető.
            thread = new Thread(Run);
        }

        // A szál azonosítója (ezt írjuk ki, és ezt adja meg a felhasználó)
        public int ThreadId
        {
            get
            {
                return thread.ManagedThreadId;
            }
        }

        // Igaz, ha a szál még fut
        public bool IsRunning
        {
            get
            {
                return thread.IsAlive;
            }
        }

        public void Start()
        {
            thread.Start();
        }

        // Megszakítás kérése: csak beállítjuk a jelzőt,
        // a szál maga fog kilépni a ciklusból.
        public void Cancel()
        {
            cancelRequested = true;
        }

        // Megvárjuk, amíg a szál befejezi a futását
        public void WaitUntilFinished()
        {
            thread.Join();
        }

        // A szál által futtatott metódus
        private void Run()
        {
            Console.WriteLine("Thread " + ThreadId + " has started.");

            int elapsedTimeInMilliseconds = 0;

            // 10 másodpercig "dolgozunk", de közben figyeljük a megszakítást
            while (elapsedTimeInMilliseconds < RunTimeInMilliseconds && cancelRequested == false)
            {
                Thread.Sleep(SleepStepInMilliseconds);
                elapsedTimeInMilliseconds = elapsedTimeInMilliseconds + SleepStepInMilliseconds;
            }

            if (cancelRequested)
            {
                Console.WriteLine("Thread " + ThreadId + " was cancelled.");
            }
            else
            {
                Console.WriteLine("Thread " + ThreadId + " has finished Running.");
            }
        }
    }
}
