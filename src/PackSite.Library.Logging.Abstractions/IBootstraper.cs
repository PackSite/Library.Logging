namespace PackSite.Library.Logging
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a bootstraper.
    /// </summary>
    public interface IBootstraper
    {
        /// <summary>
        /// Executed before instance host creation.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        void BeforeHostCreation(BootstraperOptions options);

        /// <summary>
        /// Executed before instance host creation.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        void BeforeHostBuild(IHostBuilder hostBuilder, BootstraperOptions options);

        /// <summary>
        /// Executed after building host.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        void AfterHostBuild(BootstraperOptions options);

        /// <summary>
        /// Executed before host disposal.
        /// </summary>
        /// <returns></returns>
        void BeforeHostDisposal();

        /// <summary>
        /// Executed after host disposal.
        /// </summary>
        /// <returns></returns>
        void AfterHostDisposal();

        /// <summary>
        /// Used to get <see cref="ILoggerFactory"/> instance used to log from bootstraper. When null is returned this operation is not supported.
        /// </summary>
        /// <returns></returns>
        ILoggerFactory? TryGetBootstrapLoggerFactory(BootstraperOptions options);
    }
}
