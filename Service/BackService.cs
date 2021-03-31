using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class BackService : IHostedService
    {
        private readonly ILogger<BackService> _logger;
        private readonly IBackGroundTaskService _TaskService;
        private Task _task;
        private CancellationTokenSource cancelToken;

        public BackService(ILogger<BackService> logger, IBackGroundTaskService TaskService)
        {
           // _logger = logger;
            _TaskService = TaskService;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            await _TaskService.CheckIn(stoppingToken);
            
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
