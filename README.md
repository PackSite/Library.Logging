# Library.Logging

[![CI](https://github.com/PackSite/Library.Logging/actions/workflows/CI.yml/badge.svg)](https://github.com/PackSite/Library.Logging/actions/workflows/CI.yml)
<!-- [![Coverage](https://codecov.io/gh/PackSite/Library.Logging/branch/main/graph/badge.svg?token=D0aORRMV78)](https://codecov.io/gh/PackSite/Library.Logging) -->

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Logging.Abstractions.svg?label=Logging.Abstractions)](https://nuget.org/packages/PackSite.Library.Logging.Abstractions)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Logging.Abstractions.svg?label=)](https://nuget.org/packages/PackSite.Library.Logging.Abstractions)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Logging.Microsoft.svg?label=Logging.Microsoft)](https://nuget.org/packages/PackSite.Library.Logging.Microsoft)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Logging.Microsoft.svg?label=)](https://nuget.org/packages/PackSite.Library.Logging.Microsoft)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Logging.Serilog.svg?label=Logging.Serilog)](https://nuget.org/packages/PackSite.Library.Logging.Serilog)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Logging.Serilog.svg?label=)](https://nuget.org/packages/PackSite.Library.Logging.Serilog)


**PackSite.Library.Logging** is a set of **.NET 5** and **.NET 6** compatible libraries that speed up logging setup and Generic Host bootstrapping with Serilog and `Microsoft.Extensions.Logging`.

The libraries simplify and provide a generic solution to the problem described in an article [Bootstrap logging with Serilog + ASP.NET Core](https://nblumhardt.com/2020/10/bootstrap-logger/).

 > (...)
 > Errors during application start-up are some of the nastiest problems to hit in production. 
 > Deployment issues like broken manifests or missing assemblies, incorrect settings, exceptions thrown during IoC container configuration or in the constructors of important components -
 > these can bring start-up to a screeching halt and cause a process exit, without leaving even so much as an error page.
 > (...)
 > 
 > ~ nblumhardt (Oct 12, 2020)

## Features
  
  - Host bootstrapping that provides application logs during host build, especially when app configuration or Serilog configuration are broken.
  - Follows a principal of "always logging something even if host build, logging configuration, or host startup fails critically".
  - Simplifies integration tests by providing a simple interface for setting host/program options.
  - Simplifies **Program.cs** by eliminating the need of writing complex logger configurations and try-catch-finally blocks.
  - Simplifies Serilog configuration by providing a set of recent enrichers and sinks.
  - Supports Serilog and `Microsoft.Extensions.Logging` based bootstrapping.
  - Supports Entity Framework Migrations generation by bypassing the bootstrapper.

## Examples

See [Examples folder](https://github.com/PackSite/Library.Logging/tree/main/examples) for all library usage examples.

## Quick start

```csharp
using PackSite.Library.Logging;
using PackSite.Library.Logging.Serilog;

public class Program
{
    public static async Task Main()
    {
        await CreateBootstrapper()
            .RunAsync();
    }

    public static IBootstrapperManager CreateBootstrapper(
        Action<IConfigureBootstrapperOptions>? options = null)
    {
        return BootstrapperManagerBuilder.Create<SerilogBootstrapper>()
            .CreateHostBuilder(CreateHostBuilderWithBootstraper)
            .ConfigureOptions(options ?? (o => { }))
            .Build();
    }

    [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", 
                     Justification = "Used for generating EF Core migrations - bypassed bootstrapper.")]
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return CreateHostBuilderWithBootstraper(new BootstrapperOptions(args));
    }

    private static IHostBuilder CreateHostBuilderWithBootstraper(
        BootstrapperOptions bootstraperOptions)
    {
        return Host.CreateDefaultBuilder(bootstraperOptions.Args)
            .UseEnvironment(bootstraperOptions.EnvironmentName)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration.ConfigureWithFailSafeDefaults(context.Configuration);
            }, preserveStaticLogger: false, writeToProviders: false);
    }
}
```
