namespace PackSite.Library.Logging.Serilog
{
    using System.Reflection;
    using global::Serilog;
    using global::Serilog.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging;
    using PackSite.Library.Logging.Serilog.Internal;

    /// <summary>
    /// Convenience helper for application bootstraping with logging in case of fatal error.
    /// </summary>
    public sealed class SerilogBootstraper : IBootstraper
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SerilogBootstraper"/>.
        /// </summary>
        public SerilogBootstraper()
        {

        }

        void IBootstraper.BeforeHostCreation(BootstraperOptions options)
        {
            /*
             * Initializes bootstrap Serilog logger for startup purposes.
             * Configuration is read from "appsettings.json" and "appsettings.{environmentName}.json", as well as
             * optional "{additionalFiles}.json" and "{additionalFiles}.{environmentName}.json",
             * and environment variables.
             */

            IConfigurationRoot configurationRoot = BootstraperConfigurationHelper.GetConfigurationRoot(options);

            LoggerConfiguration loggerConfiguration = new();
            loggerConfiguration.ConfigureSerilogCommons(configurationRoot, Assembly.GetEntryAssembly()?.GetName().Name ?? "Bootstrap", options.EnvironmentName);
            loggerConfiguration.MinimumLevel.Verbose(); // Log everything before logger reconfiguration by Host (unless namespace log level is overriden in appsettings.json etc.).

            ReloadableLogger logger = loggerConfiguration.CreateBootstrapLogger();
            Log.Logger = logger;
        }

        void IBootstraper.BeforeHostBuild(IHostBuilder hostBuilder, BootstraperOptions options)
        {

        }

        void IBootstraper.AfterHostBuild(BootstraperOptions options)
        {

        }

        void IBootstraper.BeforeHostDisposal()
        {
            Log.CloseAndFlush();
        }

        void IBootstraper.AfterHostDisposal()
        {

        }

        Microsoft.Extensions.Logging.ILoggerFactory? IBootstraper.TryGetBootstrapLoggerFactory(BootstraperOptions options)
        {
            return SerilogStaticLoggerHelper.CreateLoggerFactory();
        }
    }
}
