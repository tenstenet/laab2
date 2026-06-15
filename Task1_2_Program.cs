using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

class Program
{
    static List<string> log = new List<string>();

    static object logLock = new object();

    static Mutex totalMutex = new Mutex();

    static Semaphore semaphore = new Semaphore(3, 3);

    static long totalSum = 0;

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();

        string[] lines = File.ReadAllLines("data.txt");

        Thread[] threads = new Thread[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            int index = i;

            threads[i] = new Thread(() =>
            {
                ProcessSet(lines[index], index + 1);
            });

            threads[i].Start();
        }

        foreach (Thread t in threads)
            t.Join();

        sw.Stop();

        Console.WriteLine("Результаты:");

        foreach (var item in log)
            Console.WriteLine(item);

        Console.WriteLine($"\nОбщий итог: {totalSum}");
        Console.WriteLine($"Время: {sw.ElapsedMilliseconds} мс");
    }

    static void ProcessSet(string line, int setNumber)
    {
        semaphore.WaitOne();

        try
        {
            string[] nums = line.Split(' ');

            int sum = 0;

            foreach (string n in nums)
                sum += int.Parse(n);

            string result = $"Набор {setNumber}, сумма = {sum}, поток {Thread.CurrentThread.ManagedThreadId}";

            lock (logLock)
            {
                log.Add(result);
            }

            totalMutex.WaitOne();

            try
            {
                totalSum += sum;
            }
            finally
            {
                totalMutex.ReleaseMutex();
            }
        }
        finally
        {
            semaphore.Release();
        }
    }
}
