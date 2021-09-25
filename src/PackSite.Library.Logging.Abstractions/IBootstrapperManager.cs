namespace PackSite.Library.Logging
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Represents a bootstrapper manager.
    /// </summary>
    public interface IBootstrapperManager
    {
        /// <summary>
        /// Bootstrapper options.
        /// </summary>
        BootstrapperOptions Options { get; }

        /// <summary>
        /// Host instance or null when failed to build.
        /// </summary>
        IHost? Host { get; }

        /// <summary>
        /// Whether host is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Attempts to runs host inside try-catch-finally with logging.
        /// Returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="token">The token to trigger shutdown.</param>
        Task RunAsync(CancellationToken token = default);

        /// <summary>
        /// Attempts to starts the host inside try-catch-finally with logging.
        /// Returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        Task StartAsync(CancellationToken token = default);

        /// <summary>
        /// Waits for host shutdown inside try-catch-finally with logging.
        /// Returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        Task WaitForShutdownAsync(CancellationToken token = default);

        /// <summary>
        /// Attempts to stops the host inside try-catch-finally with logging.
        /// Returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        Task StopAsync(CancellationToken token = default);
    }

    /// <summary>
    /// Represents a bootstrapper manager.
    /// </summary>
    public interface IBootstrapperManager<TBootstrapper> : IBootstrapperManager
        where TBootstrapper : class, IBootstrapper
    {

    }
}
