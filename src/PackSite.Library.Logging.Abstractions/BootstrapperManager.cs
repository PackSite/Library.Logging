namespace PackSite.Library.Logging
{
    using System;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Bootstrapper manager helpers.
    /// </summary>
    public static class BootstrapperManager
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
        /// Creates a bootstrapper manager that uses <typeparamref name="TBootstrapper"/> instance as a bootstrapper.
        /// </summary>
        /// <typeparam name="TBootstrapper"></typeparam>
        /// <returns></returns>
        public static IBootstrapperManager<TBootstrapper> Create<TBootstrapper>()
            where TBootstrapper : class, IBootstrapper, new()
        {
            return new BootstrapperManager<TBootstrapper>(new TBootstrapper());
        }

        /// <summary>
        /// Creates a bootstrapper manager that uses <typeparamref name="TBootstrapper"/> instance as a bootstrapper.
        /// </summary>
        /// <typeparam name="TBootstrapper"></typeparam>
        /// <returns></returns>
        public static IBootstrapperManager<TBootstrapper> Create<TBootstrapper>(TBootstrapper instance)
            where TBootstrapper : class, IBootstrapper
        {
            return new BootstrapperManager<TBootstrapper>(instance);
        }
    }
}
