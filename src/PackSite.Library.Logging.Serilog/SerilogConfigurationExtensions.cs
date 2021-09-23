namespace PackSite.Library.Logging.Serilog
{
    using System;
    using global::Serilog;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging.Serilog.Internal;

    /// <summary>
    /// Serilog configuration extensions.
    /// </summary>
    public static class SerilogConfigurationExtensions
    {
        /// <summary>
        /// Adds Serilog logging to appliation with logger options from app configuration.
        /// Additionally, the following enrichers are always set:
        /// "App" property with appliaction name,
        /// "Env" property with environment name,
        /// "Ver" property with entry assembly version,
        /// "FromLogContext", and
        /// "WithExceptionDetails".
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
        /// <returns></returns>
        public static IHostBuilder UseLogging(this IHostBuilder builder)
        {
            return builder.UseLogging((context, services, loggerConfiguration) => { });
        }

        /// <summary>
        /// Adds Serilog logging to appliation with logger options from app configuration.
        /// Additionally, the following enrichers are always set:
        /// "App" property with appliaction name,
        /// "Env" property with environment name,
        /// "Ver" property with entry assembly version,
        /// "FromLogContext", and
        /// "WithExceptionDetails".
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
        /// <returns></returns>
        public static IHostBuilder UseLogging(this IHostBuilder builder, Action<HostBuilderContext, LoggerConfiguration> configureLogger)
        {
            return builder.UseLogging((context, services, loggerConfiguration) => configureLogger(context, loggerConfiguration));
        }

        /// <summary>
        /// Adds Serilog logging to appliation with logger options from app configuration.
        /// Additionally, the following enrichers are always set:
        /// "App" property with appliaction name,
        /// "Env" property with environment name,
        /// "Ver" property with entry assembly version,
        /// "FromLogContext", and
        /// "WithExceptionDetails".
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
        /// <returns></returns>
        public static IHostBuilder UseLogging(this IHostBuilder builder, Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogger)
        {
            builder.UseSerilog((context, services, loggerConfiguration) =>
            {
                IConfiguration configuration = context.Configuration;
                IHostEnvironment environment = context.HostingEnvironment;

                loggerConfiguration.ConfigureSerilogCommons(configuration, environment.ApplicationName, environment.EnvironmentName);
                configureLogger(context, services, loggerConfiguration);
            }, false, false);

            return builder;
        }
    }
}
