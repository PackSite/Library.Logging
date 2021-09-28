namespace PackSite.Library.Logging.Serilog.Internal
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using global::Serilog;
    using global::Serilog.Events;
    using Microsoft.Extensions.Configuration;
    using PackSite.Library.Logging.Serilog;

    /// <summary>
    /// Serilog configuration extensions.
    /// </summary>
    internal static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Configures Serilog.
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="configuration"></param>
        /// <param name="configurationSectionName"></param>
        public static void ConfigureSerilogCommons(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, string configurationSectionName = "Serilog")
        {
            configurationSectionName ??= "Serilog";

            if (configuration.GetSection(configurationSectionName)?.GetChildren().Any() is null or false)
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

                string message = $"Application configuration does not contain \"{configurationSectionName}\" section. Fallback configuration is used.";

                Debug.WriteLine(message);
                Trace.WriteLine(message);
                Console.WriteLine(message);

                ILogger logger = Log.Logger.ForContext(typeof(SerilogConfigurationExtensions));
                logger.Error(message);
                logger.Warning("Fallback configuration will write to {Console} and {Debug} with {Level} level.", typeof(Console).FullName, typeof(Debug).FullName, LogEventLevel.Verbose);
            }
            else
            {
                loggerConfiguration
                    .ReadFrom.Configuration(configuration);
            }

            loggerConfiguration
                .Enrich.FromLogContext();
        }
    }
}
