# Technology Stack

## Framework & Runtime
- .NET 9.0
- ASP.NET Core Web API
- C# with nullable reference types enabled
- Implicit usings enabled

## Key Dependencies
- **Serilog** - Structured logging framework
  - Serilog.AspNetCore
  - Serilog.Sinks.File, PostgreSQL, Seq, Grafana.Loki, Http
- **FluentValidation** - Input validation
- **Swashbuckle/OpenAPI** - API documentation
- **xUnit** - Unit testing framework
- **FluentAssertions** - Test assertions
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing

## Build System
- MSBuild with SDK-style project files
- Visual Studio solution structure
- NuGet package management

## Common Commands

### Build
```bash
dotnet build
dotnet build --configuration Release
```

### Test
```bash
dotnet test
dotnet test --collect:"XPlat Code Coverage"
```

### Run
```bash
dotnet run --project CleanObservability.Demo
```

### Package Management
```bash
dotnet restore
dotnet add package [PackageName]
```

## Development Environment
- Supports Visual Studio 2022
- Docker support for observability stack (Grafana, Loki, Seq)
- PostgreSQL for log storage
- HTTPS redirection enabled