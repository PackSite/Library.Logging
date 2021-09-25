namespace PackSite.Library.Logging
{
    using System;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Represents a bootstrapper manager.
    /// </summary>
    public interface IBootstrapperManagerBuilder<TBootstrapper>
        where TBootstrapper : class, IBootstrapper
    {
        /// <summary>
        /// Configures bootstrapper options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IBootstrapperManagerBuilder<TBootstrapper> ConfigureOptions(Action<IConfigureBootstrapperOptions> options);

        /// <summary>
        /// Sets a delegate that creates and configures host builder. Subsequent calls are not chained.
        /// </summary>
        /// <param name="createHostBuilder"></param>
        /// <returns></returns>
        IBootstrapperManagerBuilder<TBootstrapper> CreateHostBuilder(Func<BootstrapperOptions, IHostBuilder> createHostBuilder);

        /// <summary>
        /// Attempts to builds host inside try-catch-finally with logging.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws when no host builder factory was set.</exception>
        IBootstrapperManager<TBootstrapper> Build();
    }
}
