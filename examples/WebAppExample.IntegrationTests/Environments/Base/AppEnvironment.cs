namespace WebAppExample.IntegrationTests.Environments.Base
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Logging;

    public abstract class AppEnvironment : BaseEnvironment
    {
        private IBootstrapperManager? BootstrapperManager { get; set; }

        protected AppEnvironment()
        {

        }

        /// <summary>
        /// Creates a new services scope.
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateServicesScope()
        {
            return BootstrapperManager?.Host?.Services.GetRequiredService<IServiceScopeFactory>().CreateScope()
                ?? throw new NullReferenceException("Host wasn't build correctly or bootstrapper manager wasn't set.");
        }

        /// <inheritdoc/>
        protected override async ValueTask InitializeAsync()
        {
            BootstrapperManager = ConfigureBootstrapperManager();
            await BootstrapperManager.StartAsync();
        }

        /// <summary>
        /// Use this method to configure bootstrapper manager.
        /// </summary>
        protected abstract IBootstrapperManager ConfigureBootstrapperManager();

        /// <inheritdoc/>
        protected override async ValueTask DisposeAsync()
        {
            if (BootstrapperManager is not null)
            {
                await BootstrapperManager.StopAsync();
            }
        }
    }
}