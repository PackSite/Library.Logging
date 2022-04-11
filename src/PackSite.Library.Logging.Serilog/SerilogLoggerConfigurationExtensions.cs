namespace PackSite.Library.Logging.Serilog
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using global::Serilog;
    using global::Serilog.Events;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// <see cref="LoggerConfiguration"/> extensions.
    /// </summary>
    public static class SerilogLoggerConfigurationExtensions
    {
        /// <summary>
        /// Configures Serilog.
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="configuration"></param>
        /// <param name="configurationSectionName"></param>
        public static void ConfigureWithFailSafeDefaults(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, string configurationSectionName = "Serilog")
        {
            configurationSectionName ??= "Serilog";

            if (configuration.GetSection(configurationSectionName)?.GetChildren().Any() is null or false)
            {
                ConfigureFailSafe(loggerConfiguration, configurationSectionName);
            }
            else
            {
                try
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(configuration, configurationSectionName);
                }
                catch (Exception ex)
                {
                    LoggerConfiguration failSafeConfiguration = new();
                    ConfigureFailSafe(failSafeConfiguration, configurationSectionName);

                    using var logger = failSafeConfiguration.CreateLogger();
                    ILogger contextLogger = logger.ForContext(typeof(SerilogConfigurationExtensions));

                    contextLogger.Fatal(ex, "Failed to read configuration from IConfiguration.");
                    contextLogger.Fatal("Unable to FailSafe!");
                    contextLogger.Fatal("Rethrowing exception!");

                    throw;
                }
            }

            loggerConfiguration
                .Enrich.FromLogContext();
        }

        private static void ConfigureFailSafe(LoggerConfiguration loggerConfiguration, string configurationSectionName)
        {
            loggerConfiguration
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}")
                .WriteTo.Debug(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}")
                .WriteTo.File($"logs\\fallback-log-.log",
                              buffered: true,
                              flushToDiskInterval: TimeSpan.FromSeconds(1),
                              rollingInterval: RollingInterval.Day,
                              outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}");

            string message = $"Application configuration does not contain \"{configurationSectionName}\" section or section is invalid. Fallback configuration is used.";

            Debug.WriteLine(message);
            Trace.WriteLine(message);
            Console.WriteLine(message);

            ILogger logger = Log.Logger.ForContext(typeof(SerilogConfigurationExtensions));
            logger.Error(message);
            logger.Warning("Fallback configuration will write to {Console} and {Debug} with {Level} level.", typeof(Console).FullName, typeof(Debug).FullName, LogEventLevel.Verbose);
        }
    }
}
