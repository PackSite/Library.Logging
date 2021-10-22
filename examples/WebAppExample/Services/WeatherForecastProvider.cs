namespace WebAppExample.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using WebAppExample.Models;

    public class WeatherForecastProvider : IWeatherForecastProvider
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IRandomizer _randomizer;

        public WeatherForecastProvider(IRandomizer randomier)
        {
            _randomizer = randomier;
        }

        public Task<IReadOnlyCollection<WeatherForecast>> GetAsync(CancellationToken cancellationToken = default)
        {
            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = _randomizer.Next(-20, 55),
                Summary = Summaries[_randomizer.Next(0, Summaries.Length)]
            })
            .ToArray();

            return Task.FromResult<IReadOnlyCollection<WeatherForecast>>(data);
        }
    }
}
