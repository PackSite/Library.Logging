namespace SampleApp
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging;
    using PackSite.Library.Logging.Microsoft;
    using PackSite.Library.Logging.Serilog;

    public class Program
    {
        public static async Task Main()
        {
            await BootstrapperManager.Create<SerilogBootstrapper>()
                .CreateHostBuilder(CreateHostBuilderSerilog)
                .StartAsync();

            await BootstrapperManager.Create<MicrosoftBootstrapper>()
                .CreateHostBuilder(CreateHostBuilderMicrosoft)
                .StartAsync();
        }

        private static IHostBuilder CreateHostBuilderSerilog(BootstrapperOptions bootstrapperOptions)
        {
            return Host
                .CreateDefaultBuilder(bootstrapperOptions.Args)
                .UseEnvironment(bootstrapperOptions.EnvironmentName)
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    throw new NotImplementedException();
                })
                .UseLogging();
        }

        private static IHostBuilder CreateHostBuilderMicrosoft(BootstrapperOptions bootstrapperOptions)
        {
            return Host
                .CreateDefaultBuilder(bootstrapperOptions.Args)
                .UseEnvironment(bootstrapperOptions.EnvironmentName)
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddHostedService<SampleAppHostedService>();
                })
                .UseLogging();
        }
    }
}
