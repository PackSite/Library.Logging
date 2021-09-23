namespace PackSite.Library.Logging
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a bootstrapper.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Executed before instance host creation.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        void BeforeHostCreation(BootstrapperOptions options);

        /// <summary>
        /// Executed before instance host creation.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        void BeforeHostBuild(IHostBuilder hostBuilder, BootstrapperOptions options);

        /// <summary>
        /// Executed after building host.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        void AfterHostBuild(BootstrapperOptions options);

        /// <summary>
        /// Executed before host disposal.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        void BeforeHostDisposal(BootstrapperOptions options);

        /// <summary>
        /// Executed after host disposal.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        void AfterHostDisposal(BootstrapperOptions options);

        /// <summary>
        /// Used to get <see cref="ILoggerFactory"/> instance used to log from bootstrapper. When null is returned this operation is not supported.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        ILoggerFactory? TryGetBootstrapLoggerFactory(BootstrapperOptions options);
    }
}
