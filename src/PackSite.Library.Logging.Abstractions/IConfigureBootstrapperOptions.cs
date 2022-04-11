namespace PackSite.Library.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Extensions.Configuration;
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
        /// Configures configuration files etc. base directory to default - <see cref="Directory.GetCurrentDirectory()"/>.
        /// </summary>
        /// <returns></returns>
        IConfigureBootstrapperOptions UseDefaultBaseDirectory();

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
        /// Configures additional bootstrapper configuration files, e.g. appsettings-logging.json
        /// used to retrieve <see cref="IConfigurationRoot"/> instance (default: empty array).
        /// </summary>
        /// <param name="additionalFiles"></param>
        /// <returns></returns>
        [Obsolete("This method will be removed in PackSite.Library.Logging 2.0.0. Use ConfigureBootstrapperConfiguration(Action<IConfigurationBuilder> configure) instead.")]
        IConfigureBootstrapperOptions UseAdditionalBootstrapperConfigurationFiles(params string[] additionalFiles);

        /// <summary>
        /// Configures bootstrapper configuration.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        IConfigureBootstrapperOptions ConfigureBootstrapperConfiguration(Action<BootstrapperOptions, IConfigurationBuilder> configure);

        /// <summary>
        /// Optional configuration action that can be executed just before building host from e.g. tests context (default: null).
        /// Subsequent calls are chained.
        /// </summary>
        /// <param name="preBuild"></param>
        /// <returns></returns>
        IConfigureBootstrapperOptions UsePreBuild(Action<IHostBuilder> preBuild);
    }
}
