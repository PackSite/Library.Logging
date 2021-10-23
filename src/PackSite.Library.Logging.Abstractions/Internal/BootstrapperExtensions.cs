namespace PackSite.Library.Logging.Internal
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// <see cref="IBootstrapper"/> extensions.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Get boot logger with category <see cref="Constants.BootrapperLoggerCategoryName"/> or default.
        /// </summary>
        /// <param name="bootstrapper"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ILogger? GetBootLoggerOrDefault(this IBootstrapper bootstrapper, BootstrapperOptions options)
        {
            return bootstrapper.TryGetBootstrapLoggerFactory(options)?.CreateLogger(Constants.BootrapperLoggerCategoryName);
        }
    }
}
