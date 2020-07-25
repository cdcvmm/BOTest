using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main()
        {
            var client = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true,
                CheckCertificateRevocationList = false
            }) {BaseAddress = new Uri("https://localhost:5001")};

            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            const int interval = 500;
            var task = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(interval, token);
                    var response = await client.GetAsync("processes", token);
                    
                    Console.Clear();
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Invalid code {response.StatusCode}");
                        continue;
                    }
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var processes =
                        JsonSerializer.Deserialize<IEnumerable<ProcessInfo>>(
                            await response.Content.ReadAsStringAsync(), options);
                    
                    foreach (var process in processes.OrderByDescending(x => x.CpuUsage).Take(20))
                    {
                        Console.WriteLine($"{process.ProcessId}    {process.ProcessName}    {process.MemoryUsage / 1024 / 1024}MB    {process.CpuUsage}ms");
                    }
                }
            }, token);

            while (true)
            {
                var key = Console.ReadKey();
                if (key.KeyChar != 'q') continue;
                cancellationTokenSource.Cancel();
                break;
            }
            task.GetAwaiter().GetResult();
        }

        private struct ProcessInfo
        {
            public int ProcessId { get; set; }
            
            public string ProcessName { get; set; }
            
            public long MemoryUsage { get; set; }
            
            public double CpuUsage { get; set; }
        }
    }
}