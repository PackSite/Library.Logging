namespace PackSite.Library.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base bootstrapper.
    /// </summary>
    internal sealed class BootstrapperManagerBuilder<TBootstrapper> : IBootstrapperManagerBuilder<TBootstrapper>, IConfigureBootstrapperOptions
        where TBootstrapper : class, IBootstrapper
    {
        private const string LoggerCategoryName = "PackSite.Library.Logging.Boot";

        private readonly TBootstrapper _instance;

        private string[]? _args;
        private string? _baseDirectory;
        private string? _environmentName;
        private string[]? _additionalFiles;
        private readonly List<Action<IHostBuilder>> _preBuild = new();
        private Func<BootstrapperOptions, IHostBuilder>? _createHostBuilderDelegate;

        /// <inheritdoc/>
        public IHost? Host { get; }

        /// <summary>
        /// A central location for sharing state during bootstrap.
        /// </summary>
        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        /// <summary>
        /// Initializes a new instance of <see cref="BootstrapperManagerBuilder{TBootstrapper}"/>.
        /// </summary>
        /// <param name="instance"></param>
        public BootstrapperManagerBuilder(TBootstrapper instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance), "Non-nullable argument cannot be null.");
        }

        public IBootstrapperManagerBuilder<TBootstrapper> ConfigureOptions(Action<IConfigureBootstrapperOptions> options)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options), "Non-nullable argument cannot be null.");
            options(this);

            return this;
        }

        public IBootstrapperManagerBuilder<TBootstrapper> CreateHostBuilder(Func<BootstrapperOptions, IHostBuilder> createHostBuilder)
        {
            _createHostBuilderDelegate = createHostBuilder ?? throw new ArgumentNullException(nameof(createHostBuilder), "Non-nullable argument cannot be null.");

            return this;
        }

        #region IConfigureBootstrapperOptions
        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseArgs(string[] args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseBaseDirectory(string baseDirectory)
        {
            _baseDirectory = baseDirectory ?? throw new ArgumentNullException(nameof(baseDirectory));
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseDefaultEnvironmentName()
        {
            _environmentName = BootstrapperManagerBuilder.DefaultEnvironmentName;
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseEnvironmentName(string environmentName)
        {
            _environmentName = environmentName ?? throw new ArgumentNullException(nameof(environmentName));
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseAdditionalLoggingConfigurationFiles(string[] additionalFiles)
        {
            _additionalFiles = additionalFiles ?? throw new ArgumentNullException(nameof(additionalFiles));
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UsePreBuild(Action<IHostBuilder> preBuild)
        {
            _ = preBuild ?? throw new ArgumentNullException(nameof(preBuild));

            _preBuild.Add(preBuild);

            return this;
        }
        #endregion

        /// <inheritdoc/>
        public IBootstrapperManager<TBootstrapper> Build()
        {
            _ = _createHostBuilderDelegate ?? throw new InvalidOperationException($"Cannot start host when {nameof(CreateHostBuilder)} was not called.");

            BootstrapperOptions options = new(_args,
                                             _baseDirectory,
                                             _environmentName,
                                             _additionalFiles,
                                             new Dictionary<object, object>(Properties));

            _instance.BeforeHostCreation(options);

            ILogger? logger = null;
            IHost? host = null;

            try
            {
                // Build host
                logger = _instance.TryGetBootstrapLoggerFactory(options)?.CreateLogger(LoggerCategoryName);
                logger?.LogInformation("Building {App} host (env: {Env})...", options.ApplicationName, options.EnvironmentName);

                IHostBuilder hostBuilder = _createHostBuilderDelegate(options);

                foreach (Action<IHostBuilder> action in _preBuild)
                {
                    action(hostBuilder);
                }

                _instance.BeforeHostBuild(hostBuilder, options);
                host = hostBuilder.Build();

                logger = _instance.TryGetBootstrapLoggerFactory(options)?.CreateLogger(LoggerCategoryName);
                logger?.LogDebug("Host built successfully");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Host builder terminated unexpectedly.");
                Trace.WriteLine(ex);

                logger?.LogCritical(ex, "Host builder terminated unexpectedly");
            }

            return new BootstrapperManager<TBootstrapper>(_instance, options, host);
        }
    }
}
