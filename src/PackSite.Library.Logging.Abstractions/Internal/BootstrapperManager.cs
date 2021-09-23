namespace PackSite.Library.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base bootstrapper.
    /// </summary>
    internal sealed class BootstrapperManager<TBootstrapper> : IBootstrapperManager<TBootstrapper>, IConfigureBootstrapperOptions
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

        /// <summary>
        /// A central location for sharing state during bootstrap.
        /// </summary>
        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        /// <summary>
        /// Initializes a new instance of <see cref="BootstrapperManager{TBootstrapper}"/>.
        /// </summary>
        /// <param name="instance"></param>
        public BootstrapperManager(TBootstrapper instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance), "Non-nullable argument cannot be null.");
        }

        public IBootstrapperManager<TBootstrapper> ConfigureOptions(Action<IConfigureBootstrapperOptions> options)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options), "Non-nullable argument cannot be null.");
            options(this);

            return this;
        }

        public IBootstrapperManager<TBootstrapper> CreateHostBuilder(Func<BootstrapperOptions, IHostBuilder> createHostBuilder)
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
            _environmentName = BootstrapperManager.DefaultEnvironmentName;
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
        public async Task StartAsync(CancellationToken token = default)
        {
            // Set default when missing
            _args ??= Environment.GetCommandLineArgs().Skip(1).ToArray();
            _baseDirectory ??= Directory.GetCurrentDirectory();
            _environmentName ??= BootstrapperManager.DefaultEnvironmentName;
            _additionalFiles ??= Array.Empty<string>();
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

                foreach (var action in _preBuild)
                {
                    action(hostBuilder);
                }

                _instance.BeforeHostBuild(hostBuilder, options);
                host = hostBuilder.Build();

                logger = _instance.TryGetBootstrapLoggerFactory(options)?.CreateLogger(LoggerCategoryName);
                logger?.LogDebug("Host built successfully");

                // Start host
                logger?.LogInformation("Starting application {App} {Ver} (env: {Env})...", options.ApplicationName, options.ApplicationVersion, options.EnvironmentName);

                _instance.AfterHostBuild(options);

                await host.RunAsync(token);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Host terminated unexpectedly.");
                Trace.WriteLine(ex);

                logger?.LogCritical(ex, "Host terminated unexpectedly");
            }
            finally
            {
                logger?.LogWarning("Application {App} (env: {Env}) closed.", options.ApplicationVersion, options.EnvironmentName);

                _instance.BeforeHostDisposal(options);
                host?.Dispose();
                _instance.AfterHostDisposal(options);
            }
        }
    }
}
