using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ProcessApi.Models;

namespace ProcessApi.Core
{
    public class CalculationHostedService : IHostedService
    {
        private Timer _timer;
        private readonly IProcessInformationStorage _processInformationStorage;
        private readonly IDictionary<int, TimeSpan> _previousTotalProcessorTimes = new Dictionary<int, TimeSpan>();

        public CalculationHostedService(IProcessInformationStorage processInformationStorage)
        {
            _processInformationStorage = processInformationStorage;
        }

        private void CalculateAndSetNewProcessInformation(object state)
        {
            var processes = Process.GetProcesses()
                .Where(x => x.Id != 0)
                .Where(x => !x.HasExited)
                .Select(x =>
                {
                    if (!_previousTotalProcessorTimes.TryGetValue(x.Id, out var previousTime))
                    {
                        previousTime = TimeSpan.Zero;
                    }
                    else
                    {
                        _previousTotalProcessorTimes.Remove(x.Id);
                    }
                    _previousTotalProcessorTimes.Add(x.Id, x.TotalProcessorTime);
                    
                    var cpuUsage = (x.TotalProcessorTime - previousTime).TotalMilliseconds;
                    return new ProcessInfo
                    {
                        ProcessId = x.Id,
                        ProcessName = x.ProcessName,
                        MemoryUsage = x.WorkingSet64,
                        CpuUsage = cpuUsage
                    };
                })
                .OrderByDescending(x => x.CpuUsage)
                .ThenByDescending(x => x.MemoryUsage)
                .ToArray();
            _processInformationStorage.SetProcessesInformation(processes);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CalculateAndSetNewProcessInformation, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken) => await _timer.DisposeAsync();
    }
}