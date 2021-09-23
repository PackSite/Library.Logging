namespace PackSite.Library.Logging
{
    using System;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Bootstraper manager helpers.
    /// </summary>
    public static class BootstraperManager
    {
        private const string DotNetEnvironment = "DOTNET_ENVIRONMENT";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// Default environment name.
        /// </summary>
        public static string DefaultEnvironmentName { get; } =
            Environment.GetEnvironmentVariable(DotNetEnvironment) ??
            Environment.GetEnvironmentVariable(AspNetCoreEnvironment) ??
            Environments.Production;

        /// <summary>
        /// Creates a core bootstraper that uses <typeparamref name="TBootstraper"/> instance as a bootstraper.
        /// </summary>
        /// <typeparam name="TBootstraper"></typeparam>
        /// <returns></returns>
        public static IBootstraperManager<TBootstraper> Create<TBootstraper>()
            where TBootstraper : class, IBootstraper, new()
        {
            return new BootstraperManager<TBootstraper>(new TBootstraper());
        }

        /// <summary>
        /// Creates a core bootstraper that uses <typeparamref name="TBootstraper"/> instance as a bootstraper.
        /// </summary>
        /// <typeparam name="TBootstraper"></typeparam>
        /// <returns></returns>
        public static IBootstraperManager<TBootstraper> Create<TBootstraper>(TBootstraper instance)
            where TBootstraper : class, IBootstraper
        {
            return new BootstraperManager<TBootstraper>(instance);
        }
    }
}
