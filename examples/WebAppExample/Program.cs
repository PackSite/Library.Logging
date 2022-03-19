namespace WebAppExample
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging;
    using PackSite.Library.Logging.Serilog;
    using Serilog;

    /// <summary>
    /// WebAppExample application entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        public static async Task Main()
        {
            await CreateBootstrapper()
                .RunAsync();
        }

        /// <summary>
        /// Boots application
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IBootstrapperManager CreateBootstrapper(Action<IConfigureBootstrapperOptions>? options = null)
        {
            return BootstrapperManagerBuilder.Create<SerilogBootstrapper>()
                .CreateHostBuilder(CreateHostBuilderWithBootstraper)
                .ConfigureOptions(options ?? (o => { }))
                .Build();
        }

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "For compatibility - used for generating EF Core migrations etc.")]
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return CreateHostBuilderWithBootstraper(new BootstrapperOptions(args));
        }

        private static IHostBuilder CreateHostBuilderWithBootstraper(BootstrapperOptions bootstraperOptions)
        {
            return Host.CreateDefaultBuilder(bootstraperOptions.Args)
                .UseEnvironment(bootstraperOptions.EnvironmentName)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (bootstraperOptions.Args.Contains("--throw"))
                    {
                        throw new InvalidOperationException("Host build exception demo");
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration.ConfigureWithFailSafeDefaults(context.Configuration);
                }, false, false);
        }
    }
}
