namespace PackSite.Library.Logging.Internal
{
    using System;
    using System.Diagnostics;
    using System.Linq;
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
        /// If an error occurred during configuration building, an empty configuration will be returned.
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot GetConfigurationRoot(BootstrapperOptions options)
        {
            string environmentName = options.EnvironmentName;

            try
            {
                IConfigurationBuilder preSettingsBuilder = new ConfigurationBuilder()
                    .SetBasePath(options.BaseDirectory)
                    .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);

#pragma warning disable CS0618 // Type or member is obsolete
                foreach (string file in options.AdditionalBootstrapperConfigurationFiles)
                {
                    string f = file.EndsWith(".json") ? file[..^5] : file;

                    if (f.Any(x => x == '.'))
                    {
                        throw new ApplicationException($"Filename ({file}) must contain only one dot ('.').");
                    }

                    preSettingsBuilder
                        .AddJsonFile(string.Concat(f, ".json"), optional: true, reloadOnChange: false)
                        .AddJsonFile(string.Concat(f, ".", environmentName, ".json"), optional: true, reloadOnChange: false);
                }
#pragma warning restore CS0618 // Type or member is obsolete

                preSettingsBuilder.AddEnvironmentVariables();

                options.Configure(preSettingsBuilder);

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
