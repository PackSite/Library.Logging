namespace PackSite.Library.Logging
{
    using System;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Logging.Internal;

    /// <summary>
    /// Bootstrapper manager builder helpers.
    /// </summary>
    public static class BootstrapperManagerBuilder
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
        public static IBootstrapperManagerBuilder<TBootstrapper> Create<TBootstrapper>()
            where TBootstrapper : class, IBootstrapper, new()
        {
            return new BootstrapperManagerBuilder<TBootstrapper>(new TBootstrapper());
        }

        /// <summary>
        /// Creates a bootstrapper manager that uses <typeparamref name="TBootstrapper"/> instance as a bootstrapper.
        /// </summary>
        /// <typeparam name="TBootstrapper"></typeparam>
        /// <returns></returns>
        public static IBootstrapperManagerBuilder<TBootstrapper> Create<TBootstrapper>(TBootstrapper instance)
            where TBootstrapper : class, IBootstrapper
        {
            return new BootstrapperManagerBuilder<TBootstrapper>(instance);
        }
    }
}
