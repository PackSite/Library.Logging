namespace PackSite.Library.Logging.Microsoft
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using global::Microsoft.Extensions.Configuration;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::Microsoft.Extensions.Hosting;
    using global::Microsoft.Extensions.Logging;
    using PackSite.Library.Logging;

    /// <summary>
    /// Application bootstraping with Microsoft.Extensions.Logging-based logging in case of fatal error.
    /// </summary>
    public sealed class MicrosoftBootstrapper : IBootstrapper
    {
        private struct Properties
        {
            /// <summary>
            /// A property name that stores Microsoft bootstrapper service provider.
            /// </summary>

            public const string BootstrapperServiceProvider = "PackSite.Library.Logging.Microsoft.MicrosoftBootstrapper.ServiceProvider";
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MicrosoftBootstrapper"/>.
        /// </summary>
        public MicrosoftBootstrapper()
        {

        }

        void IBootstrapper.BeforeHostCreation(BootstrapperOptions options, IConfigurationRoot bootstrapperConfigurationRoot)
        {
            /*
             * Initializes bootstrap MEL logger for startup purposes.
             * Configuration is read from "appsettings.json" and "appsettings.{environmentName}.json", as well as
             * optional "{additionalFiles}.json" and "{additionalFiles}.{environmentName}.json",
             * and environment variables.
             */

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddOptions()
                .AddLogging(logging =>
                {
                    // Log everything before logger reconfiguration by Host (unless namespace log level is overriden in appsettings.json etc.).
                    logging.AddFilter(level => level >= LogLevel.Trace);

                    var section = bootstrapperConfigurationRoot.GetSection("Logging");
                    if (section?.GetChildren().Any() is true)
                    {
                        logging.AddConfiguration(section);
                    }
                    else
                    {
                        const string message = "Application configuration does not contain \"Logging\" section. Fallback configuration will be used.";

                        Debug.WriteLine(message);
                        Trace.WriteLine(message);
                        Console.WriteLine(message);
                    }

#if NET6_0_OR_GREATER
                    if (!OperatingSystem.IsBrowser())
#endif
                    {
                        logging.AddConsole();
                    }
                    logging.AddDebug();
                    logging.AddEventSourceLogger();

                    logging.Configure(options =>
                    {
                        options.ActivityTrackingOptions =
                            ActivityTrackingOptions.SpanId |
                            ActivityTrackingOptions.TraceId |
                            ActivityTrackingOptions.ParentId;
                    });
                })
                .BuildServiceProvider();

            options.Properties[Properties.BootstrapperServiceProvider] = serviceProvider;
        }

        void IBootstrapper.BeforeHostBuild(IHostBuilder hostBuilder, BootstrapperOptions options, IConfigurationRoot bootstrapperConfigurationRoot)
        {

        }

        void IBootstrapper.AfterHostDisposal(BootstrapperOptions options)
        {
            if (options.Properties.TryGetValue(Properties.BootstrapperServiceProvider, out object? value) &&
                value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        ILoggerFactory? IBootstrapper.TryGetBootstrapLoggerFactory(BootstrapperOptions options)
        {
            if (options.Properties.TryGetValue(Properties.BootstrapperServiceProvider, out object? value) &&
                value is IServiceProvider serviceProvider)
            {
                return serviceProvider.GetService<ILoggerFactory>();
            }

            return null;
        }
    }
}
