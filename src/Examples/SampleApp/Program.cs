namespace SampleApp
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging;
    using PackSite.Library.Logging.Serilog;

    public class Program
    {
        public static async Task Main()
        {
            await BootstraperManager.Create<SerilogBootstraper>()
                .CreateHostBuilder(CreateHostBuilder)
                .StartAsync();
        }

        private static IHostBuilder CreateHostBuilder(BootstraperOptions bootstraperOptions)
        {
            return Host
                .CreateDefaultBuilder(bootstraperOptions.Args)
                .UseEnvironment(bootstraperOptions.EnvironmentName)
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();

                    services.AddHostedService<SampleAppHostedService>();
                })
                .UseLogging();
        }
    }
}
