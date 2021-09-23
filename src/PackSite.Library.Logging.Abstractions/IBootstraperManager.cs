namespace PackSite.Library.Logging
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Represents a bootstraper manager.
    /// </summary>
    public interface IBootstraperManager<TBootstraper>
        where TBootstraper : class, IBootstraper
    {
        /// <summary>
        /// Configures bootstraper options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IBootstraperManager<TBootstraper> ConfigureOptions(Action<IConfigureBootstraperOptions> options);

        /// <summary>
        /// Sets a delegate that creates and configures host builder. Subsequent calls are not chained.
        /// </summary>
        /// <param name="createHostBuilder"></param>
        /// <returns></returns>
        IBootstraperManager<TBootstraper> CreateHostBuilder(Func<BootstraperOptions, IHostBuilder> createHostBuilder);

        /// <summary>
        /// Builds and runs host inside try-catch-finally with logging.
        /// Returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws when no host builder factory was set.</exception>
        Task StartAsync(CancellationToken token = default);
    }
}
