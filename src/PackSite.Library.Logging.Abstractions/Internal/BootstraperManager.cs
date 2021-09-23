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
    /// Base bootstraper.
    /// </summary>
    internal sealed class BootstraperManager<TBootstraper> : IBootstraperManager<TBootstraper>, IConfigureBootstraperOptions
        where TBootstraper : class, IBootstraper
    {
        private const string LoggerCategoryName = "PackSite.Library.Logging.Boot";

        private readonly TBootstraper _instance;

        private string[]? _args;
        private string? _baseDirectory;
        private string? _environmentName;
        private string[]? _additionalFiles;
        private readonly List<Action<IHostBuilder>> _preBuild = new();
        private Func<BootstraperOptions, IHostBuilder>? _createHostBuilderDelegate;

        /// <summary>
        /// Initializes a new instance of <see cref="BootstraperManager{TBootstraper}"/>.
        /// </summary>
        /// <param name="instance"></param>
        public BootstraperManager(TBootstraper instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance), "Non-nullable argument cannot be null.");
        }

        public IBootstraperManager<TBootstraper> ConfigureOptions(Action<IConfigureBootstraperOptions> options)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options), "Non-nullable argument cannot be null.");
            options(this);

            return this;
        }

        public IBootstraperManager<TBootstraper> CreateHostBuilder(Func<BootstraperOptions, IHostBuilder> createHostBuilder)
        {
            _createHostBuilderDelegate = createHostBuilder ?? throw new ArgumentNullException(nameof(createHostBuilder), "Non-nullable argument cannot be null.");

            return this;
        }

        #region IConfigureBootstraperOptions
        IConfigureBootstraperOptions IConfigureBootstraperOptions.UseArgs(string[] args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
            return this;
        }

        IConfigureBootstraperOptions IConfigureBootstraperOptions.UseBaseDirectory(string baseDirectory)
        {
            _baseDirectory = baseDirectory ?? throw new ArgumentNullException(nameof(baseDirectory));
            return this;
        }

        IConfigureBootstraperOptions IConfigureBootstraperOptions.UseDefaultEnvironmentName()
        {
            _environmentName = BootstraperManager.DefaultEnvironmentName;
            return this;
        }

        IConfigureBootstraperOptions IConfigureBootstraperOptions.UseEnvironmentName(string environmentName)
        {
            _environmentName = environmentName ?? throw new ArgumentNullException(nameof(environmentName));
            return this;
        }

        IConfigureBootstraperOptions IConfigureBootstraperOptions.UseAdditionalLoggingConfigurationFiles(string[] additionalFiles)
        {
            _additionalFiles = additionalFiles ?? throw new ArgumentNullException(nameof(additionalFiles));
            return this;
        }

        IConfigureBootstraperOptions IConfigureBootstraperOptions.UsePreBuild(Action<IHostBuilder> preBuild)
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
            _environmentName ??= BootstraperManager.DefaultEnvironmentName;
            _additionalFiles ??= Array.Empty<string>();
            _ = _createHostBuilderDelegate ?? throw new InvalidOperationException($"Cannot start host when {nameof(CreateHostBuilder)} was not called.");

            BootstraperOptions options = new(_args, _baseDirectory, _environmentName, _additionalFiles);
            _instance.BeforeHostCreation(options);

            ILogger? logger = null;
            IHost? host = null;

            try
            {
                // Build host
                logger = _instance.TryGetBootstrapLoggerFactory(options)?.CreateLogger(LoggerCategoryName);
                logger?.LogInformation("Building {App} host (env: {Env})...");

                IHostBuilder hostBuilder = _createHostBuilderDelegate(options);

                foreach (var action in _preBuild)
                {
                    action(hostBuilder);
                }

                _instance.BeforeHostBuild(hostBuilder, options);
                host = hostBuilder.Build();

                logger = _instance.TryGetBootstrapLoggerFactory(options)?.CreateLogger(LoggerCategoryName);
                logger?.LogDebug("Host built successfully");

                // Start host.
                logger?.LogInformation("Starting application {App} {Ver} (env: {Env})...");

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
                logger?.LogWarning("Application {App} closed.");

                _instance.BeforeHostDisposal();
                host?.Dispose();
                _instance.AfterHostDisposal();
            }
        }
    }
}
