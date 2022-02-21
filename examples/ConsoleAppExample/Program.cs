namespace ConsoleAppExample
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging;
    using PackSite.Library.Logging.Microsoft;
    using PackSite.Library.Logging.Serilog;
    using Serilog;

    public class Program
    {
        public static async Task Main()
        {
            System.Console.WriteLine("===== DEMO #1 =====");
            await BootstrapperManagerBuilder.Create<SerilogBootstrapper>()
                .CreateHostBuilder(CreateHostBuilderSerilog)
                .Build()
                .RunAsync();

            System.Console.WriteLine("===== DEMO #2 =====");
            var bootstrapperManagerMicrosoft = BootstrapperManagerBuilder.Create<MicrosoftBootstrapper>()
                .CreateHostBuilder(CreateHostBuilderMicrosoft)
                .Build();

            await bootstrapperManagerMicrosoft.StartAsync();
            await bootstrapperManagerMicrosoft.WaitForShutdownAsync();
        }

        private static IHostBuilder CreateHostBuilderSerilog(BootstrapperOptions bootstrapperOptions)
        {
            return Host
                .CreateDefaultBuilder(bootstrapperOptions.Args)
                .UseEnvironment(bootstrapperOptions.EnvironmentName)
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddHostedService<SampleAppHostedService1>();
                    services.AddHostedService<SampleAppHostedService2>();
                })
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration.ConfigureWithFailSafeDefaults(context.Configuration);
                }, false, false);
        }

        private static IHostBuilder CreateHostBuilderMicrosoft(BootstrapperOptions bootstrapperOptions)
        {
            return Host
                .CreateDefaultBuilder(bootstrapperOptions.Args)
                .UseEnvironment(bootstrapperOptions.EnvironmentName)
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddHostedService<SampleAppHostedService1>();
                    services.AddHostedService<SampleAppHostedService2>();
                });
        }
    }
}
