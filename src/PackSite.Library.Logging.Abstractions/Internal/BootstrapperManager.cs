namespace PackSite.Library.Logging.Internal
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base bootstrapper.
    /// </summary>
    internal sealed class BootstrapperManager<TBootstrapper> : IBootstrapperManager<TBootstrapper>
        where TBootstrapper : class, IBootstrapper
    {
        private readonly TBootstrapper _bootstrapper;

        /// <inheritdoc/>
        public BootstrapperOptions Options { get; }

        /// <inheritdoc/>
        public IHost? Host { get; }

        /// <inheritdoc/>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BootstrapperManager{TBootstrapper}"/>.
        /// </summary>
        /// <param name="bootstrapper"></param>
        /// <param name="options"></param>
        /// <param name="host"></param>
        public BootstrapperManager(TBootstrapper bootstrapper, BootstrapperOptions options, IHost? host)
        {
            _bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper), "Non-nullable argument cannot be null.");
            Options = options ?? throw new ArgumentNullException(nameof(options), "Non-nullable argument cannot be null.");
            Host = host;
        }

        /// <inheritdoc/>
        public async Task RunAsync(CancellationToken token = default)
        {
            if (Host is null)
            {
                return;
            }

            try
            {
                _bootstrapper
                    .GetBootLoggerOrDefault(Options)
                    ?.LogInformation("Starting application {App} {Ver} (env: {Env})...", Options.ApplicationName, Options.ApplicationVersion, Options.EnvironmentName);

                IsRunning = true;
                await Host.RunAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Host terminated unexpectedly.");
                Trace.WriteLine(ex);

                _bootstrapper
                    .GetBootLoggerOrDefault(Options)
                    ?.LogCritical(ex, "Host terminated unexpectedly");
            }
            finally
            {
                _bootstrapper
                    .GetBootLoggerOrDefault(Options)
                    ?.LogWarning("Application {App} (env: {Env}) closed.", Options.ApplicationVersion, Options.EnvironmentName);

                // No need to dispose host because RunAsync does that
                IsRunning = false;
                _bootstrapper.AfterHostDisposal(Options);
            }
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken token = default)
        {
            if (Host is null)
            {
                return;
            }

            try
            {
                ILogger? logger = _bootstrapper.GetBootLoggerOrDefault(Options);
                logger?.LogInformation("Starting application {App} {Ver} (env: {Env})...", Options.ApplicationName, Options.ApplicationVersion, Options.EnvironmentName);

                IsRunning = true;
                await Host.StartAsync(token).ConfigureAwait(false);

                logger?.LogWarning("Application {App} (env: {Env}) started.", Options.ApplicationVersion, Options.EnvironmentName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Host terminated unexpectedly during startup.");
                Trace.WriteLine(ex);

                _bootstrapper
                    .GetBootLoggerOrDefault(Options)
                    ?.LogCritical(ex, "Host terminated unexpectedly during startup");

                await StopAsync(token).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken token = default)
        {
            if (Host is null || !IsRunning)
            {
                return;
            }

            try
            {
                _bootstrapper
                    .GetBootLoggerOrDefault(Options)
                    ?.LogInformation("Stopping application {App} {Ver} (env: {Env})...", Options.ApplicationName, Options.ApplicationVersion, Options.EnvironmentName);

                await Host.StopAsync(token);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Host terminated unexpectedly.");
                Trace.WriteLine(ex);

                _bootstrapper
                    .GetBootLoggerOrDefault(Options)?
                    .LogCritical(ex, "Host terminated unexpectedly");
            }
            finally
            {
                _bootstrapper
                    .GetBootLoggerOrDefault(Options)
                    ?.LogWarning("Application {App} (env: {Env}) stopped.", Options.ApplicationVersion, Options.EnvironmentName);

                if (Host is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                }
                else
                {
                    Host?.Dispose();
                }

                IsRunning = false;
                _bootstrapper.AfterHostDisposal(Options);
            }
        }

        /// <inheritdoc/>
        public async Task StopAsync(TimeSpan timeout)
        {
            using CancellationTokenSource cts = new(timeout);
            await StopAsync(cts.Token).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task WaitForShutdownAsync(CancellationToken token = default)
        {
            if (Host is null || !IsRunning)
            {
                return;
            }

            try
            {
                await Host.WaitForShutdownAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Host terminated unexpectedly.");
                Trace.WriteLine(ex);

                _bootstrapper
                    .GetBootLoggerOrDefault(Options)
                    ?.LogCritical(ex, "Host terminated unexpectedly");
            }
            finally
            {
                _bootstrapper
                    .GetBootLoggerOrDefault(Options)
                    ?.LogWarning("Application {App} (env: {Env}) stopped.", Options.ApplicationVersion, Options.EnvironmentName);

                if (Host is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                }
                else
                {
                    Host?.Dispose();
                }

                IsRunning = false;
                _bootstrapper.AfterHostDisposal(Options);
            }
        }
    }
}
