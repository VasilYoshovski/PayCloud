using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace PayCloud.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Seeded the database.");

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddAzureWebAppDiagnostics();
                //logging.AddNlog();
                //logging.AddSerilog();
            })
            .ConfigureServices(serviceCollection => serviceCollection
            .Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "azure-diagnostics-";
                options.FileSizeLimit = 50 * 1024;
                options.RetainedFileCountLimit = 5;
            })
            .Configure<AzureBlobLoggerOptions>(options =>
            {
                options.BlobName = "log.txt";
            }))
            .UseStartup<Startup>()
            .Build();
    }
}
