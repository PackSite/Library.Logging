namespace PackSite.Library.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// <see cref="IBootstrapper"/> options.
    /// </summary>
    public sealed class BootstrapperOptions
    {
        /// <summary>
        /// Application name.
        /// </summary>
        public string ApplicationName { get; } = Assembly.GetEntryAssembly()?.GetName().Name ?? "Bootstrap";

        /// <summary>
        /// Application version.
        /// </summary>
        public Version ApplicationVersion { get; } = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(1, 0, 0, 0);

        /// <summary>
        /// CLI args (default: Environment.GetCommandLineArgs().Skip(1).ToArray()).
        /// </summary>
        public string[] Args { get; }

        /// <summary>
        /// Configuration files etc. base directory (default: <see cref="Directory.GetCurrentDirectory()"/>).
        /// </summary>
        public string BaseDirectory { get; }

        /// <summary>
        /// Environment name.
        /// </summary>
        public string EnvironmentName { get; }

        /// <summary>
        /// Additional bootstrapper configuration files, e.g. appsettings-logging (environment specific files are added automatically)
        /// used to retrieve <see cref="IConfigurationRoot"/> instance (default: empty array).
        /// </summary>
        [Obsolete("This property will be removed in PackSite.Library.Logging 2.0.0. Use ConfigureBootstrapperConfiguration instead.")]
        public string[] AdditionalBootstrapperConfigurationFiles { get; }

        /// <summary>
        /// A list of delegates to configure bootstrapper configuration.
        /// </summary>
        public IReadOnlyList<Action<BootstrapperOptions, IConfigurationBuilder>> ConfigureBootstrapperConfiguration { get; }

        /// <summary>
        /// A central location for sharing state during bootstrap.
        /// </summary>
        public IDictionary<object, object> Properties { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="BootstrapperOptions"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="baseDirectory"></param>
        /// <param name="environmentName"></param>
        /// <param name="additionalLoggingConfigurationFiles"></param>
        /// <param name="configureBootstrapperConfiguration"></param>
        /// <param name="properties"></param>
        public BootstrapperOptions(string[]? args = null,
                                   string? baseDirectory = null,
                                   string? environmentName = null,
                                   string[]? additionalLoggingConfigurationFiles = null,
                                   IReadOnlyList<Action<BootstrapperOptions, IConfigurationBuilder>>? configureBootstrapperConfiguration = null,
                                   IDictionary<object, object>? properties = null)
        {
            Args = args ?? Environment.GetCommandLineArgs().Skip(1).ToArray();
            BaseDirectory = baseDirectory ?? Directory.GetCurrentDirectory();
            EnvironmentName = environmentName ?? BootstrapperManagerBuilder.DefaultEnvironmentName;
            ConfigureBootstrapperConfiguration = configureBootstrapperConfiguration ?? new List<Action<BootstrapperOptions, IConfigurationBuilder>>();
            Properties = properties ?? new Dictionary<object, object>();

#pragma warning disable CS0618 // Type or member is obsolete
            AdditionalBootstrapperConfigurationFiles = additionalLoggingConfigurationFiles ?? Array.Empty<string>();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Configures <see cref="ConfigurationBuilder"/> instance.
        /// </summary>
        /// <param name="configurationBuilder"></param>
        public void Configure(IConfigurationBuilder configurationBuilder)
        {
            foreach (Action<BootstrapperOptions, IConfigurationBuilder> action in ConfigureBootstrapperConfiguration)
            {
                action(this, configurationBuilder);
            }
        }
    }
}
