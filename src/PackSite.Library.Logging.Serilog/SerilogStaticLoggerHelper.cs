namespace PackSite.Library.Logging.Serilog
{
    using System;
    using Microsoft.Extensions.Logging;
    using S = global::Serilog;

    /// <summary>
    /// Logger utils useful inside ConfigureAppConfiguration or Startup class.
    /// </summary>
    public static class SerilogStaticLoggerHelper
    {
        /// <summary>
        /// Gets a <see cref="ILoggerFactory"/> from static Serilog logger.
        /// </summary>
        /// <returns></returns>
        public static ILoggerFactory CreateLoggerFactory()
        {
            // MEL logger factory
            LoggerFactory loggerFactory = new();
            S.SerilogLoggerFactoryExtensions.AddSerilog(loggerFactory, S.Log.Logger, dispose: false);

            return loggerFactory; // We're not disposing static logger so it's safe not to care about LoggerFactory.Dispose.
        }

        /// <summary>
        /// Creates a <see cref="ILoggerFactory"/> from static Serilog logger.
        /// </summary>
        /// <returns></returns>
        public static ILogger<T> CreateLogger<T>()
        {
            // MEL logger factory
            ILoggerFactory loggerFactory = CreateLoggerFactory();
            ILogger<T> logger = loggerFactory.CreateLogger<T>();

            return logger;
        }

        /// <summary>
        /// Creates a <see cref="ILoggerFactory"/> from static Serilog logger.
        /// </summary>
        /// <returns></returns>
        public static ILogger CreateLogger(Type type)
        {
            // MEL logger factory
            ILoggerFactory loggerFactory = CreateLoggerFactory();
            ILogger logger = loggerFactory.CreateLogger(type);

            return logger;
        }

        /// <summary>
        /// Creates a <see cref="ILoggerFactory"/> from static Serilog logger.
        /// </summary>
        /// <returns></returns>
        public static ILogger CreateLogger(string categoryName)
        {
            // MEL logger factory
            ILoggerFactory loggerFactory = CreateLoggerFactory();
            ILogger logger = loggerFactory.CreateLogger(categoryName);

            return logger;
        }
    }
}
