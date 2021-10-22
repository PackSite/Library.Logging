namespace WebAppExample.IntegrationTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using WebAppExample.IntegrationTests.Environments;
    using WebAppExample.Services;
    using Xunit;

    [Collection(WebAppExampleEnvironmentCollection.Name)]
    public class WeatherForecastProviderTests
    {
        private readonly WebAppExampleEnvironment _fixture;

        public WeatherForecastProviderTests(WebAppExampleEnvironment fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_return_5_entries_with_min_temperature()
        {
            using IServiceScope scope = _fixture.CreateServicesScope();
            var provider = scope.ServiceProvider.GetRequiredService<IWeatherForecastProvider>();

            var data = await provider.GetAsync();

            data.Should().HaveCount(5);
            data.Select(x => x.TemperatureC).Distinct().Should().BeEquivalentTo(new[] { -20 });
        }
    }
}
