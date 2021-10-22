namespace WebAppExample.IntegrationTests.Environments.Base
{
    using System.Threading.Tasks;
    using Xunit;

    public abstract class BaseEnvironment : IAsyncLifetime
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BaseEnvironment"/>
        /// </summary>
        protected BaseEnvironment()
        {

        }

        /// <summary>
        /// Use this method to initialize the environment.
        /// </summary>
        /// <returns></returns>
        protected abstract ValueTask InitializeAsync();

        /// <summary>
        /// Use this method to dispose the environment.
        /// </summary>
        /// <returns></returns>
        protected abstract ValueTask DisposeAsync();

        async Task IAsyncLifetime.InitializeAsync()
        {
            await InitializeAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await DisposeAsync();
        }
    }
}