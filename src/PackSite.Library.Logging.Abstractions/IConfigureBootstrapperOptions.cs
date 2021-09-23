namespace PackSite.Library.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Configure bootstrapper options.
    /// </summary>
    public interface IConfigureBootstrapperOptions
    {
        /// <summary>
        /// A central location for sharing state during bootstrap.
        /// </summary>
        IDictionary<object, object> Properties { get; }

        /// <summary>
        /// Configures CLI args (default: Environment.GetCommandLineArgs().Skip(1).ToArray()).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IConfigureBootstrapperOptions UseArgs(string[] args);

        /// <summary>
        /// Configures configuration files etc. base directory (default: <see cref="Directory.GetCurrentDirectory()"/>).
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <returns></returns>
        IConfigureBootstrapperOptions UseBaseDirectory(string baseDirectory);

        /// <summary>
        /// Configures host environment to default.
        /// </summary>
        /// <returns></returns>
        IConfigureBootstrapperOptions UseDefaultEnvironmentName();

        /// <summary>
        /// Configures host environment.
        /// </summary>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        IConfigureBootstrapperOptions UseEnvironmentName(string environmentName);

        /// <summary>
        /// Configures additional logging configuration files, e.g. appsettings-logging.Development.json (default: empty array).
        /// </summary>
        /// <param name="additionalFiles"></param>
        /// <returns></returns>
        IConfigureBootstrapperOptions UseAdditionalLoggingConfigurationFiles(string[] additionalFiles);

        /// <summary>
        /// Optional services configuration action that can be executed just before building host from e.g. tests context (default: null).
        /// Subsequent calls are chained.
        /// </summary>
        /// <param name="preBuild"></param>
        /// <returns></returns>
        IConfigureBootstrapperOptions UsePreBuild(Action<IHostBuilder> preBuild);
    }
}
