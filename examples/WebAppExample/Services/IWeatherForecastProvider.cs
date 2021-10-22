namespace WebAppExample.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using WebAppExample.Models;

    public interface IWeatherForecastProvider
    {
        Task<IReadOnlyCollection<WeatherForecast>> GetAsync(CancellationToken cancellationToken = default);
    }
}