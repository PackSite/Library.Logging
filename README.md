# Library.Logging

[![CI](https://github.com/PackSite/Library.Logging/actions/workflows/CI.yml/badge.svg)](https://github.com/PackSite/Library.Logging/actions/workflows/CI.yml)
[![Coverage](https://codecov.io/gh/PackSite/Library.Logging/branch/main/graph/badge.svg?token=D0aORRMV78)](https://codecov.io/gh/PackSite/Library.Logging)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Logging.Abstractions.svg?label=Logging.Abstractions)](https://nuget.org/packages/PackSite.Library.Logging.Abstractions)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Logging.Abstractions.svg?label=)](https://nuget.org/packages/PackSite.Library.Logging.Abstractions)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Logging.Microsoft.svg?label=Logging.Microsoft)](https://nuget.org/packages/PackSite.Library.Logging.Microsoft)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Logging.Microsoft.svg?label=)](https://nuget.org/packages/PackSite.Library.Logging.Microsoft)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Logging.Serilog.svg?label=Logging.Serilog)](https://nuget.org/packages/PackSite.Library.Logging.Serilog)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Logging.Serilog.svg?label=)](https://nuget.org/packages/PackSite.Library.Logging.Serilog)


**PackSite.Library.Logging** is a **.NET 5** library that speeds up logging setup and Host bootstrapping with Serilog and `Microsoft.Extensions.Logging`.

The library simplifies and provides a generic solution to the problem described in an article [Bootstrap logging with Serilog + ASP.NET Core](https://nblumhardt.com/2020/10/bootstrap-logger/).

 > (...)
 > Errors during application start-up are some of the nastiest problems to hit in production. 
 > Deployment issues like broken manifests or missing assemblies, incorrect settings, exceptions thrown during IoC container configuration or in the constructors of important components -
 > these can bring start-up to a screeching halt and cause a process exit, without leaving even so much as an error page.
 > (...)


## Features
  
  - Host bootstrapping that provides application logs during host build, especially when app configuration or Serilog configuration are broken.
  - Simplifies integration tests by providing a simple interface for setting host/program options.
  - Simplifies Program.cs.

## Examples

See [Examples folder](https://github.com/PackSite/Library.Logging/tree/main/examples) for all library usage examples.