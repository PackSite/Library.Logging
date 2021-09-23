namespace PackSite.Library.Logging
{
    using System;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Represents a core bootstraper.
    /// </summary>
    public interface ICoreBootstraperBuilder
    {
        /// <summary>
        /// Builds and runs host inside try-catch-finally with logging. Additionally, when debugger is attached a debugger break will be called.
        /// Logger Configuration is read from "appsettings.json" and "appsettings.{environmentName}.json", as well as
        /// optional "{additionalFiles}.json" and "{additionalFiles}.{environmentName}.json",
        /// and environment variables.
        /// </summary>
        /// <param name="createHostBuilder"></param>
        void BuildAndRun(Func<BootstraperOptions, IHostBuilder> createHostBuilder);
    }
}
