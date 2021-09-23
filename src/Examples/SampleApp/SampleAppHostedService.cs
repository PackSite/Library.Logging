namespace SampleApp
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

    public sealed class SampleAppHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public SampleAppHostedService(ILogger<SampleAppHostedService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting SampleApp");

            IReadOnlyDictionary<string, string> values = _configuration.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
            string json = JsonConvert.SerializeObject(values, Formatting.Indented);

            Console.WriteLine(json);

            throw new NotImplementedException();

            return Task.CompletedTask;
        }
    }
}
