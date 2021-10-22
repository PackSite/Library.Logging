namespace WebAppExample.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using WebAppExample.Models;
    using WebAppExample.Services;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<WeatherForecast>> GetAsync([FromServices] IWeatherForecastProvider provider, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get invoked");

            return await provider.GetAsync(cancellationToken);
        }
    }
}
