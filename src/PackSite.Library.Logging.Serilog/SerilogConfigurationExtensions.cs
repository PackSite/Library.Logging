namespace PackSite.Library.Logging.Serilog
{
    using System;
    using global::Serilog;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Serilog configuration extensions.
    /// </summary>
    public static class SerilogConfigurationExtensions
    {
        /// <summary>
        /// Adds Serilog logging to application with logger options from app configuration.
        /// Additionally, the following enrichers are always set:
        /// "FromLogContext".
        ///
        /// Optional enrichers:
        /// "WithMemoryUsage",
        /// "WithProcessId,
        /// "WithProcessName",
        /// "WithThreadId",
        /// "WithThreadName",
        /// "WithMachineName",
        /// "WithEnvironmentUserName", and
        /// "WithEnvironmentName",
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configurationSectionName"></param>
        /// <param name="writeToProviders">
        /// By default, Serilog does not write events to Microsoft.Extensions.Logging.ILoggerProviders
        /// registered through the Microsoft.Extensions.Logging API. Normally, equivalent
        /// Serilog sinks are used in place of providers. Specify true to write events to all providers.
        /// </param>
        /// <returns></returns>
        [Obsolete(
            $"This extension will be removed in PackSite.Library.Logging 2.0.0, " +
            $"please use {nameof(SerilogLoggerConfigurationExtensions)}.{nameof(SerilogLoggerConfigurationExtensions.ConfigureWithFailSafeDefaults)}(...) instead.")]
        public static IHostBuilder UseLogging(this IHostBuilder builder,
                                              string configurationSectionName = "Serilog",
                                              bool writeToProviders = false)
        {
            return builder.UseLogging((context, services, loggerConfiguration) => { }, configurationSectionName, writeToProviders);
        }

        /// <summary>
        /// Adds Serilog logging to application with logger options from app configuration.
        /// Additionally, the following enrichers are always set:
        /// "FromLogContext".
        ///
        /// Optional enrichers:
        /// "WithMemoryUsage",
        /// "WithProcessId,
        /// "WithProcessName",
        /// "WithThreadId",
        /// "WithThreadName",
        /// "WithMachineName",
        /// "WithEnvironmentUserName", and
        /// "WithEnvironmentName",
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureLogger"></param>
        /// <param name="configurationSectionName"></param>
        /// <param name="writeToProviders">
        /// By default, Serilog does not write events to Microsoft.Extensions.Logging.ILoggerProviders
        /// registered through the Microsoft.Extensions.Logging API. Normally, equivalent
        /// Serilog sinks are used in place of providers. Specify true to write events to all providers.
        /// </param>
        /// <returns></returns>
        [Obsolete(
            $"This extension will be removed in PackSite.Library.Logging 2.0.0, " +
            $"please use {nameof(SerilogLoggerConfigurationExtensions)}.{nameof(SerilogLoggerConfigurationExtensions.ConfigureWithFailSafeDefaults)}(...) instead.")]
        public static IHostBuilder UseLogging(this IHostBuilder builder,
                                              Action<HostBuilderContext, LoggerConfiguration> configureLogger,
                                              string configurationSectionName = "Serilog",
                                              bool writeToProviders = false)
        {
            return builder.UseLogging((context, services, loggerConfiguration) =>
            {
                configureLogger(context, loggerConfiguration);
            }, configurationSectionName, writeToProviders);
        }

        /// <summary>
        /// Adds Serilog logging to application with logger options from app configuration.
        /// Additionally, the following enrichers are always set:
        /// "FromLogContext".
        ///
        /// Optional enrichers:
        /// "WithMemoryUsage",
        /// "WithProcessId,
        /// "WithProcessName",
        /// "WithThreadId",
        /// "WithThreadName",
        /// "WithMachineName",
        /// "WithEnvironmentUserName", and
        /// "WithEnvironmentName",
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureLogger"></param>
        /// <param name="configurationSectionName"></param>
        /// <param name="writeToProviders">
        /// By default, Serilog does not write events to Microsoft.Extensions.Logging.ILoggerProviders
        /// registered through the Microsoft.Extensions.Logging API. Normally, equivalent
        /// Serilog sinks are used in place of providers. Specify true to write events to all providers.
        /// </param>
        /// <returns></returns>
        [Obsolete(
            $"This extension will be removed in PackSite.Library.Logging 2.0.0, " +
            $"please use {nameof(SerilogLoggerConfigurationExtensions)}.{nameof(SerilogLoggerConfigurationExtensions.ConfigureWithFailSafeDefaults)}(...) instead.")]
        public static IHostBuilder UseLogging(this IHostBuilder builder,
                                              Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogger,
                                              string configurationSectionName = "Serilog",
                                              bool writeToProviders = false)
        {
            builder.UseSerilog((context, services, loggerConfiguration) =>
            {
                IConfiguration configuration = context.Configuration;
                IHostEnvironment environment = context.HostingEnvironment;

                loggerConfiguration.ConfigureWithFailSafeDefaults(configuration, configurationSectionName);
                configureLogger(context, services, loggerConfiguration);
            }, false, writeToProviders);

            return builder;
        }
    }
}
