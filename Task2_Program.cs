// Версия 1: Синхронная
using System;
using System.Net.Http;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();

        HttpClient client = new HttpClient();

        string[] urls =
        {
            "https://jsonplaceholder.typicode.com/posts/1",
            "https://jsonplaceholder.typicode.com/users/1",
            "https://jsonplaceholder.typicode.com/todos/1"
        };

        foreach (string url in urls)
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                    return;
                }

                string json = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(json);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        sw.Stop();

        Console.WriteLine($"Время работы: {sw.ElapsedMilliseconds} мс");
    }
}

/* Версия 2: Асинхронная
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

class Program
{
    static async Task Main()
    {
        Stopwatch sw = Stopwatch.StartNew();

        HttpClient client = new HttpClient();

        string[] urls =
        {
            "https://jsonplaceholder.typicode.com/posts/1",
            "https://jsonplaceholder.typicode.com/users/1",
            "https://jsonplaceholder.typicode.com/todos/1"
        };

        try
        {
            Task<string>[] tasks =
            {
                client.GetStringAsync(urls[0]),
                client.GetStringAsync(urls[1]),
                client.GetStringAsync(urls[2])
            };

            string[] results = await Task.WhenAll(tasks);

            foreach (string json in results)
            {
                Console.WriteLine(json);
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        sw.Stop();

        Console.WriteLine($"Время работы: {sw.ElapsedMilliseconds} мс");
    }
}
*/
