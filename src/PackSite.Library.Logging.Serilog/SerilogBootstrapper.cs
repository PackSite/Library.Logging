namespace PackSite.Library.Logging.Serilog
{
    using global::Serilog;
    using global::Serilog.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging;

    /// <summary>
    /// Application bootstraping using Serilog-based logging in case of fatal error.
    /// </summary>
    public sealed class SerilogBootstrapper : IBootstrapper
    {
        //TODO: maybe add support for setting/reading section name from BootstrapperOptions.Properties + extensions to read/write this prop

        private readonly string _configurationSectionName = "Serilog";

        /// <summary>
        /// Initializes a new instance of <see cref="SerilogBootstrapper"/>.
        /// </summary>
        public SerilogBootstrapper()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="SerilogBootstrapper"/>.
        /// </summary>
        /// <param name="configurationSectionName"></param>
        public SerilogBootstrapper(string configurationSectionName)
        {
            _configurationSectionName = configurationSectionName ?? "Serilog";
        }

        void IBootstrapper.BeforeHostCreation(BootstrapperOptions options, IConfigurationRoot bootstrapperConfigurationRoot)
        {
            /*
             * Initializes bootstrap Serilog logger for startup purposes.
             * Configuration is read from "appsettings.json" and "appsettings.{environmentName}.json", as well as
             * optional "{additionalFiles}.json" and "{additionalFiles}.{environmentName}.json",
             * and environment variables.
             */

            string configurationSectionName = _configurationSectionName;

            LoggerConfiguration loggerConfiguration = new();
            loggerConfiguration.ConfigureWithFailSafeDefaults(bootstrapperConfigurationRoot, configurationSectionName);
            loggerConfiguration.MinimumLevel.Verbose(); // Log everything before logger reconfiguration by Host (unless namespace log level is overriden in appsettings.json etc.).

            ReloadableLogger logger = loggerConfiguration.CreateBootstrapLogger();
            Log.Logger = logger;
        }

        void IBootstrapper.BeforeHostBuild(IHostBuilder hostBuilder, BootstrapperOptions options, IConfigurationRoot bootstrapperConfigurationRoot)
        {

        }

        void IBootstrapper.AfterHostDisposal(BootstrapperOptions options)
        {
            Log.CloseAndFlush();
        }

        Microsoft.Extensions.Logging.ILoggerFactory? IBootstrapper.TryGetBootstrapLoggerFactory(BootstrapperOptions options)
        {
            return SerilogStaticLoggerHelper.CreateLoggerFactory();
        }
    }
}
