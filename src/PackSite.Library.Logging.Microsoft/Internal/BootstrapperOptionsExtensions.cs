namespace PackSite.Library.Logging.Microsoft.Internal
{
    using System;

    /// <summary>
    /// <see cref="BootstrapperOptions"/> extensions.
    /// </summary>
    public static class BootstrapperOptionsExtensions
    {
        /// <summary>
        /// Sets a service provider to a property
        /// </summary>
        /// <param name="options"></param>
        /// <param name="serviceProvider"></param>
        public static void SetServiceProvider(this BootstrapperOptions options, IServiceProvider serviceProvider)
        {
            options.Properties[InternalProperties.BootstrapperServiceProvider] = serviceProvider;
        }

        /// <summary>
        /// Sets a service provider to a property
        /// </summary>
        /// <param name="options"></param>
        public static IServiceProvider? GetServiceProviderOrDefault(this BootstrapperOptions options)
        {
            if (options.Properties.TryGetValue(InternalProperties.BootstrapperServiceProvider, out object? value) &&
                value is IServiceProvider serviceProvider)
            {
                return serviceProvider;
            }

            return null;
        }
    }
}
