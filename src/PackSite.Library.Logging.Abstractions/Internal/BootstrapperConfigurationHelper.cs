namespace PackSite.Library.Logging.Internal
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Bootstrapper configuration helper.
    /// </summary>
    internal static class BootstrapperConfigurationHelper
    {
        /// <summary>
        /// Build and gets <see cref="IConfigurationRoot"/> that can be used to configure logger for startup purposes.
        ///
        /// Configuration is read from "appsettings.json" and "appsettings.{environmentName}.json", as well as
        /// optional "{additionalFiles}.json" and "{additionalFiles}.{environmentName}.json",
        /// and environment variables.
        ///
        /// If an error occured during configuration building, an empty configuration will be returned.
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot GetConfigurationRoot(BootstrapperOptions options)
        {
            return GetConfigurationRoot(options.EnvironmentName, options.BaseDirectory, options.AdditionalBootstrapperConfigurationFiles);
        }

        private static IConfigurationRoot GetConfigurationRoot(string environmentName, string baseFolderPath, string[] additionalFiles)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Debug.WriteLine(ex);
                Trace.WriteLine(ex);

                return new ConfigurationBuilder().Build();
            }
        }
    }
}
