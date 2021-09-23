namespace PackSite.Library.Logging
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Bootstraper configuration helper.
    /// </summary>
    public static class BootstraperConfigurationHelper
    {
        /// <summary>
        /// Build and gets <see cref="IConfigurationRoot"/> that can be used to configure logger for startup purposes.
        /// Configuration is read from "appsettings.json" and "appsettings.{environmentName}.json", as well as
        /// optional "{additionalFiles}.json" and "{additionalFiles}.{environmentName}.json",
        /// and environment variables.
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot GetConfigurationRoot(BootstraperOptions options)
        {
            return GetConfigurationRoot(options.EnvironmentName, options.BaseDirectory, options.AdditionalFiles);
        }

        private static IConfigurationRoot GetConfigurationRoot(string environmentName, string baseFolderPath, string[] additionalFiles)
        {
            IConfigurationBuilder preSettingsBuilder = new ConfigurationBuilder()
                .SetBasePath(baseFolderPath)
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);

            foreach (string e in additionalFiles)
            {
                preSettingsBuilder
                    .AddJsonFile(string.Concat(e, ".json"), optional: true, reloadOnChange: false)
                    .AddJsonFile(string.Concat(e, ".", environmentName, ".json"), optional: true, reloadOnChange: false);
            }

            preSettingsBuilder.AddEnvironmentVariables();

            return preSettingsBuilder.Build();
        }
    }
}
