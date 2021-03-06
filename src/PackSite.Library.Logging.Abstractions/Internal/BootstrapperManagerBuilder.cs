namespace PackSite.Library.Logging.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base bootstrapper.
    /// </summary>
    internal sealed class BootstrapperManagerBuilder<TBootstrapper> : IBootstrapperManagerBuilder<TBootstrapper>, IConfigureBootstrapperOptions
        where TBootstrapper : class, IBootstrapper
    {
        private readonly TBootstrapper _bootstrapper;

        private string[]? _args;
        private string? _baseDirectory;
        private string? _environmentName;
        private readonly List<string> _additionalFiles = new();
        private readonly List<Action<BootstrapperOptions, IConfigurationBuilder>> _configureBootstrapperConfiguration = new();
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
        /// <param name="bootstrapper"></param>
        public BootstrapperManagerBuilder(TBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper), "Non-nullable argument cannot be null.");
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

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseDefaultBaseDirectory()
        {
            _baseDirectory = null;
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseBaseDirectory(string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                throw new ArgumentException($"'{nameof(baseDirectory)}' cannot be null or whitespace.", nameof(baseDirectory));
            }

            _baseDirectory = baseDirectory;
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseDefaultEnvironmentName()
        {
            _environmentName = null;
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseEnvironmentName(string environmentName)
        {
            if (string.IsNullOrWhiteSpace(environmentName))
            {
                throw new ArgumentException($"'{nameof(environmentName)}' cannot be null or whitespace.", nameof(environmentName));
            }

            _environmentName = environmentName;
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.UseAdditionalBootstrapperConfigurationFiles(params string[] additionalFiles)
        {
            _additionalFiles.AddRange(additionalFiles ?? throw new ArgumentNullException(nameof(additionalFiles)));
            return this;
        }

        IConfigureBootstrapperOptions IConfigureBootstrapperOptions.ConfigureBootstrapperConfiguration(Action<BootstrapperOptions, IConfigurationBuilder> configure)
        {
            _configureBootstrapperConfiguration.Add(configure);

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
                                              _additionalFiles.ToArray(),
                                              _configureBootstrapperConfiguration,
                                              new Dictionary<object, object>(Properties));

            IConfigurationRoot configurationRoot = BootstrapperConfigurationHelper.GetConfigurationRoot(options);

            _bootstrapper.BeforeHostCreation(options, configurationRoot);

            IHost? host = null;

            try
            {
                // Build host
                _bootstrapper
                    .GetBootLoggerOrDefault(options)
                    ?.LogInformation("Building {App} host (env: {Env})...", options.ApplicationName, options.EnvironmentName);

                IHostBuilder hostBuilder = _createHostBuilderDelegate(options);

                foreach (Action<IHostBuilder> action in _preBuild)
                {
                    action(hostBuilder);
                }

                _bootstrapper.BeforeHostBuild(hostBuilder, options, configurationRoot);
                host = hostBuilder.Build();

                _bootstrapper
                    .GetBootLoggerOrDefault(options)
                    ?.LogInformation("Host built successfully");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Host builder terminated unexpectedly.");
                Trace.WriteLine(ex);

                _bootstrapper
                    .GetBootLoggerOrDefault(options)
                    ?.LogCritical(ex, "Host builder terminated unexpectedly");
            }

            return new BootstrapperManager<TBootstrapper>(_bootstrapper, options, host);
        }
    }
}
