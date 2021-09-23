namespace PackSite.Library.Logging.Serilog.Internal
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using global::Serilog;
    using global::Serilog.Events;
    using global::Serilog.Exceptions;
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
        /// <param name="appName"></param>
        /// <param name="environmentName"></param>
        public static void ConfigureSerilogCommons(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, string appName, string environmentName)
        {
            if (!configuration.GetSection("Serilog").GetChildren().Any())
            {
                loggerConfiguration
                    .MinimumLevel.Verbose()
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    .WriteTo.File($"logs\\fallback-log-.log", buffered: true, flushToDiskInterval: TimeSpan.FromSeconds(1), rollingInterval: RollingInterval.Day)
                    .Enrich.FromLogContext();

                const string message = "Application configuration does not contain \"Serilog\" section. Fallback configuration will be used.";

                Debug.WriteLine(message);
                Trace.WriteLine(message);
                Console.WriteLine(message);

                ILogger logger = Log.Logger.ForContext(typeof(SerilogConfigurationExtensions));
                logger.Error("Application configuration does not contain \"Serilog\" section. Fallback configuration is used.");
                logger.Warning("Fallback configuration will write to {Console} and {Debug} with {Level} level.", typeof(Console).FullName, typeof(Debug).FullName, LogEventLevel.Verbose);
            }
            else
            {
                loggerConfiguration
                    .ReadFrom.Configuration(configuration);
            }

            loggerConfiguration
                .Enrich.WithProperty("App", appName)
                .Enrich.WithProperty("Env", environmentName)
                .Enrich.WithProperty("Ver", Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(1, 0, 0, 0))
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails();
        }
    }
}
