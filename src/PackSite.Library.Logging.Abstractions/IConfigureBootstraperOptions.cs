namespace PackSite.Library.Logging
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Configure bootstraper options.
    /// </summary>
    public interface IConfigureBootstraperOptions
    {
        /// <summary>
        /// Configures CLI args (default: Environment.GetCommandLineArgs().Skip(1).ToArray()).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IConfigureBootstraperOptions UseArgs(string[] args);

        /// <summary>
        /// Configures configuration files etc. base directory (default: <see cref="Directory.GetCurrentDirectory()"/>).
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <returns></returns>
        IConfigureBootstraperOptions UseBaseDirectory(string baseDirectory);

        /// <summary>
        /// Configures host environment to default.
        /// </summary>
        /// <returns></returns>
        IConfigureBootstraperOptions UseDefaultEnvironmentName();

        /// <summary>
        /// Configures host environment.
        /// </summary>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        IConfigureBootstraperOptions UseEnvironmentName(string environmentName);

        /// <summary>
        /// Configures additional logging configuration files, e.g. appsettings-logging.Development.json (default: empty array).
        /// </summary>
        /// <param name="additionalFiles"></param>
        /// <returns></returns>
        IConfigureBootstraperOptions UseAdditionalLoggingConfigurationFiles(string[] additionalFiles);

        /// <summary>
        /// Optional services configuration action that can be executed just before building host from e.g. tests context (default: null).
        /// Subsequent calls are chained.
        /// </summary>
        /// <param name="preBuild"></param>
        /// <returns></returns>
        IConfigureBootstraperOptions UsePreBuild(Action<IHostBuilder> preBuild);
    }
}
