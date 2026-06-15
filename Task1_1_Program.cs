using System;
using System.Threading;
using System.Diagnostics;

class Program
{
    static int primeCount = 0;
    static object locker = new object();
    static Mutex mutex = new Mutex();
    static Semaphore semaphore = new Semaphore(1, 1);

    static void Main()
    {
        Console.WriteLine("Выберите способ синхронизации:");
        Console.WriteLine("1 - Monitor (lock)");
        Console.WriteLine("2 - Mutex");
        Console.WriteLine("3 - Semaphore");
        int choice = int.Parse(Console.ReadLine());

        Stopwatch sw = Stopwatch.StartNew();

        Thread t1 = new Thread(() => CountPrimes(1, 2500, 1, choice));
        Thread t2 = new Thread(() => CountPrimes(2501, 5000, 2, choice));
        Thread t3 = new Thread(() => CountPrimes(5001, 7500, 3, choice));
        Thread t4 = new Thread(() => CountPrimes(7501, 10000, 4, choice));

        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();

        t1.Join();
        t2.Join();
        t3.Join();
        t4.Join();

        sw.Stop();

        Console.WriteLine($"\nВсего простых чисел: {primeCount}");
        Console.WriteLine($"Время: {sw.ElapsedMilliseconds} мс");
    }

    static void CountPrimes(int start, int end, int threadId, int mode)
    {
        for (int i = start; i <= end; i++)
        {
            Console.WriteLine($"Поток {threadId}: проверяю число {i}");

            if (IsPrime(i))
            {
                Console.WriteLine($"Поток {threadId}: нашёл простое число {i}");

                if (mode == 1)
                {
                    lock (locker)
                    {
                        primeCount++;
                    }
                }
                else if (mode == 2)
                {
                    mutex.WaitOne();
                    primeCount++;
                    mutex.ReleaseMutex();
                }
                else if (mode == 3)
                {
                    semaphore.WaitOne();
                    primeCount++;
                    semaphore.Release();
                }
            }
        }
    }

    static bool IsPrime(int n)
    {
        if (n < 2)
            return false;

        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
                return false;
        }

        return true;
    }
}