namespace PackSite.Library.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

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
        /// Configuration files etc. base director (default: <see cref="Directory.GetCurrentDirectory()"/>).
        /// </summary>
        public string BaseDirectory { get; }

        /// <summary>
        /// Environment name.
        /// </summary>
        public string EnvironmentName { get; }

        /// <summary>
        /// Additional logging configuration files, e.g. appsettings-logging.Development.json (default: empty array).
        /// </summary>
        public string[] AdditionalLoggingConfigurationFiles { get; }

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
        /// <param name="properties"></param>
        public BootstrapperOptions(string[] args, string baseDirectory, string environmentName, string[] additionalLoggingConfigurationFiles, IDictionary<object, object> properties)
        {
            Args = args ?? throw new ArgumentNullException(nameof(args));
            BaseDirectory = baseDirectory ?? throw new ArgumentNullException(nameof(baseDirectory));
            EnvironmentName = environmentName ?? throw new ArgumentNullException(nameof(environmentName));
            AdditionalLoggingConfigurationFiles = additionalLoggingConfigurationFiles ?? throw new ArgumentNullException(nameof(additionalLoggingConfigurationFiles));
            Properties = properties;
        }
    }
}
