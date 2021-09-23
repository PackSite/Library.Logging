namespace PackSite.Library.Logging.Serilog
{
    using global::Serilog;
    using global::Serilog.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging;
    using PackSite.Library.Logging.Serilog.Internal;

    /// <summary>
    /// Application bootstraping using Serilog-based logging in case of fatal error.
    /// </summary>
    public sealed class SerilogBootstrapper : IBootstrapper
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SerilogBootstrapper"/>.
        /// </summary>
        public SerilogBootstrapper()
        {

        }

        void IBootstrapper.BeforeHostCreation(BootstrapperOptions options)
        {
            /*
             * Initializes bootstrap Serilog logger for startup purposes.
             * Configuration is read from "appsettings.json" and "appsettings.{environmentName}.json", as well as
             * optional "{additionalFiles}.json" and "{additionalFiles}.{environmentName}.json",
             * and environment variables.
             */

            IConfigurationRoot configurationRoot = BootstrapperConfigurationHelper.GetConfigurationRoot(options);

            LoggerConfiguration loggerConfiguration = new();
            loggerConfiguration.ConfigureSerilogCommons(configurationRoot);
            loggerConfiguration.MinimumLevel.Verbose(); // Log everything before logger reconfiguration by Host (unless namespace log level is overriden in appsettings.json etc.).

            ReloadableLogger logger = loggerConfiguration.CreateBootstrapLogger();
            Log.Logger = logger;
        }

        void IBootstrapper.BeforeHostBuild(IHostBuilder hostBuilder, BootstrapperOptions options)
        {

        }

        void IBootstrapper.AfterHostBuild(BootstrapperOptions options)
        {

        }

        void IBootstrapper.BeforeHostDisposal(BootstrapperOptions options)
        {
            Log.CloseAndFlush();
        }

        void IBootstrapper.AfterHostDisposal(BootstrapperOptions options)
        {

        }

        Microsoft.Extensions.Logging.ILoggerFactory? IBootstrapper.TryGetBootstrapLoggerFactory(BootstrapperOptions options)
        {
            return SerilogStaticLoggerHelper.CreateLoggerFactory();
        }
    }
}
