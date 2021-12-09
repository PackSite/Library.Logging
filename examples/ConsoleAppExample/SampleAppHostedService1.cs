namespace ConsoleAppExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public sealed class SampleAppHostedService1 : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public SampleAppHostedService1(ILogger<SampleAppHostedService2> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);

            _logger.LogInformation($"Running SampleApp");

            IReadOnlyDictionary<string, string> values = _configuration.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
            string json = JsonConvert.SerializeObject(values, Formatting.Indented);

            Console.WriteLine(json);
        }
    }
}
