using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace BackendWorker
{
    public class MyBackgroundWorker : BackgroundService
    {
        private readonly ILogger _logger;

        public MyBackgroundWorker(ILogger logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background worker running.    " + this.ToString());

            string localDirectory = AppContext.BaseDirectory;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Doing background work...  {localDirectory}\\DailyCostTracker.exe should be there");
                await Task.Delay(5000, stoppingToken); // Example delay                               
            }

            _logger.LogInformation("Background worker stopping.");
        }
    }
}

