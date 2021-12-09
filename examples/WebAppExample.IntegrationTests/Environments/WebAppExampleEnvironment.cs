namespace WebAppExample.IntegrationTests.Environments
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PackSite.Library.Logging;
    using WebAppExample;
    using WebAppExample.IntegrationTests.Environments.Base;
    using WebAppExample.IntegrationTests.Mocks.Services;
    using WebAppExample.Services;

    public sealed class WebAppExampleEnvironment : AppEnvironment
    {
        public WebAppExampleEnvironment() : base()
        {

        }

        /// <inheritdoc/>
        protected override IBootstrapperManager ConfigureBootstrapperManager()
        {
            return Program.CreateBootstrapper(ConfigureTests);
        }

        private static void ConfigureTests(IConfigureBootstrapperOptions options)
        {
            options.UseEnvironmentName("Test");
            options.UsePreBuild((hostBuilder) =>
            {
                hostBuilder.ConfigureServices((context, services) =>
                {
                    services.RemoveAll<IRandomizer>();
                    services.AddSingleton<IRandomizer, RandomizerMock>();
                });

                hostBuilder.ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["Logging:LogLevel"] = "Warning"
                    });
                });
            });
        }
    }
}
