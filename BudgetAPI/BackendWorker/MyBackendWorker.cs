using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

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
            _logger.LogInformation(this.ToString() + ":ExecuteAsync Enter");

        //    string baseDirectory = AppContext.BaseDirectory;

        //    _logger.LogInformation(this.ToString() + ":ExecuteAsync: Background worker running baseDirectory is " + baseDirectory);

        //    string watchPath = @"C:\Downloads";

        //    string pathToBankTransactionFile = baseDirectory + Environment.GetEnvironmentVariable("PathToBankTransactionFile");

        //    if (string.IsNullOrEmpty(pathToBankTransactionFile))
        //    {
        //        _logger.LogError(this.ToString() + ":ExecuteAsync: PathToBankTransactionFile environment variable is not set .");
        //        return;
        //    }
        //    else if (!Path.Exists(pathToBankTransactionFile))
        //    {
        //        _logger.LogError(this.ToString() + $":ExecuteAsync: PathToBankTransactionFile: {pathToBankTransactionFile} does not exist.");
        //        return;
        //    }
            
        //    _logger.LogInformation(this.ToString() + $":ExecuteAsync: PathToBankTransactionFile is {pathToBankTransactionFile} environment variable.");
                
        //    long lastFileSize = 0;

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        long fileLength = 0;

        //        string[] filesToImport = Directory.GetFiles(pathToBankTransactionFile, "transaction*.csv");

        //        _logger.LogInformation(this.ToString() + ":ExecuteAsync: ImportData processing: There are {count} files to process", filesToImport.Length);

        //        if (filesToImport.Length == 1)
        //        {
        //            // there is a file, last make sure its done being downloaded.
        //            _logger.LogInformation(this.ToString() + $":ExecuteAsync: file to process is " + filesToImport[0]);

        //            FileInfo fileInfo = new FileInfo(filesToImport[0]);
        //            if (fileInfo.Length > 0)
        //            {
        //                if (fileInfo.Length > lastFileSize)
        //                {
        //                    lastFileSize = fileInfo.Length;
        //                }
        //                else if (fileInfo.Length == fileInfo.Length)
        //                {
        //                    // now the file is stabilized so run the import utlity                            
        //                    try
        //                    {
        //                        _logger.LogInformation($"Doing background work... this is where i might DailyCostTracker.exe  ");

        //                        //Process.Start(new ProcessStartInfo
        //                        //{
        //                        //    FileName = Path.Combine(baseDirectory, "DailyCostTracker.exe"),
        //                        //    Arguments = "integrateWithWebApp",
        //                        //    UseShellExecute = false,
        //                        //    RedirectStandardOutput = true,
        //                        //    RedirectStandardError = true
        //                        //});

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        _logger.LogError(ex, "Error while trying to start DailyCostTracker.exe");
        //                        return;
        //                    }
        //                }
        //            }
        //        }

                await Task.Delay(5000, stoppingToken); // Example delay                               
        }

        //    _logger.LogInformation("Background worker stopping.");
        //}
    }
}

