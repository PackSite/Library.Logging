namespace PackSite.Library.Logging
{
    using System;
    using System.IO;

    /// <summary>
    /// <see cref="IBootstraper"/> options.
    /// </summary>
    public sealed class BootstraperOptions
    {
        /// <summary>
        /// CLI args (default: Environment.GetCommandLineArgs().Skip(1).ToArray()).
        /// </summary>
        public string[] Args { get; }

        /// <summary>
        /// Configuration files etc. base director (default: <see cref="Directory.GetCurrentDirectory()"/>).
        /// </summary>
        public string BaseDirectory { get; }

        /// <summary>
        /// Bootstraper environment name.
        /// </summary>
        public string EnvironmentName { get; }

        /// <summary>
        /// Additional files, e.g. appsettings-logging.Development.json (default: empty array).
        /// </summary>
        public string[] AdditionalFiles { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="BootstraperOptions"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="baseDirectory"></param>
        /// <param name="environmentName"></param>
        /// <param name="additionalFiles"></param>
        public BootstraperOptions(string[] args, string baseDirectory, string environmentName, string[] additionalFiles)
        {
            Args = args ?? throw new ArgumentNullException(nameof(args));
            BaseDirectory = baseDirectory ?? throw new ArgumentNullException(nameof(baseDirectory));
            EnvironmentName = environmentName ?? throw new ArgumentNullException(nameof(environmentName));
            AdditionalFiles = additionalFiles ?? throw new ArgumentNullException(nameof(additionalFiles));
        }
    }
}
