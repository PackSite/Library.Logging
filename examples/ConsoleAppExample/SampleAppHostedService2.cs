namespace ConsoleAppExample
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    public sealed class SampleAppHostedService2 : BackgroundService
    {
        public SampleAppHostedService2()
        {

        }

        /// <inheritdoc/>
        [DebuggerStepThrough]
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new InvalidOperationException();
            //return Task.CompletedTask;
        }
    }
}
