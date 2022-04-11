namespace ConsoleAppWithSerilogExpressionsExample
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging;
    using PackSite.Library.Logging.Serilog;
    using Serilog;

    public class Program
    {
        public static async Task Main()
        {
            await BootstrapperManagerBuilder.Create<SerilogBootstrapper>()
                .ConfigureOptions(c =>
                {
                    c.ConfigureBootstrapperConfiguration((options, builder) =>
                    {
                        builder.Sources.Clear();
                        builder
                            .SetBasePath(options.BaseDirectory)
                            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
                            .AddEnvironmentVariables();
                    });
                })
                .CreateHostBuilder(CreateHostBuilder)
                .Build()
                .RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(BootstrapperOptions bootstrapperOptions)
        {
            return Host
                .CreateDefaultBuilder(bootstrapperOptions.Args)
                .UseEnvironment(bootstrapperOptions.EnvironmentName)
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddHostedService<SampleAppHostedService1>();
                })
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration.ConfigureWithFailSafeDefaults(context.Configuration);
                }, preserveStaticLogger: false, writeToProviders: false);
        }
    }
}
