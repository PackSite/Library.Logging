namespace ConsoleAppExample
{
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
            await BootstrapperManagerBuilder.Create<SerilogBootstrapper>()
                .CreateHostBuilder(CreateHostBuilderSerilog)
                .Build()
                .RunAsync();

            var bootstrapperManagerMicrosoft = BootstrapperManagerBuilder.Create<MicrosoftBootstrapper>()
                .CreateHostBuilder(CreateHostBuilderMicrosoft)
                .Build();

            await bootstrapperManagerMicrosoft.StartAsync();
            await bootstrapperManagerMicrosoft.WaitForShutdownAsync();
            //await bootstrapperManagerMicrosoft.StopAsync();
        }

        private static IHostBuilder CreateHostBuilderSerilog(BootstrapperOptions bootstrapperOptions)
        {
            return Host
                .CreateDefaultBuilder(bootstrapperOptions.Args)
                .UseEnvironment(bootstrapperOptions.EnvironmentName)
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddHostedService<SampleAppHostedService2>();
                    services.AddHostedService<SampleAppHostedService1>();
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
                    services.AddHostedService<SampleAppHostedService2>();
                    services.AddHostedService<SampleAppHostedService1>();
                })
                .UseLogging();
        }
    }
}
