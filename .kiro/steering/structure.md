# Project Structure

## Solution Organization
The solution follows a clean architecture pattern with three main projects:

### CleanObservability.Core
Core library containing reusable observability components:
- `Diagnostics/` - Correlation ID handling and middleware
- `Errors/` - API error definitions and registry
- `Extensions/` - Result pattern extensions
- `Result/` - Result pattern implementation and error response builders
- `Setup/` - Configuration and dependency injection extensions
- `Utilities/` - HTTP request extensions and error dispatching

### CleanObservability.Demo
Web API demonstration project:
- `Controllers/` - API controllers
- `Models/` - Request/response DTOs
- `Validators/` - FluentValidation validators
- `Middlewares/` - Custom middleware components
- `Extensions/` - Middleware registration extensions
- `docker/` - Docker compose and observability stack configuration
- `Logs/` - File-based log output (organized by year)

### CleanObservability.Tests
Test project with unit and integration tests:
- Uses xUnit testing framework
- FluentAssertions for readable test assertions
- ASP.NET Core testing utilities for integration tests

## Naming Conventions
- Projects: `CleanObservability.[Purpose]`
- Namespaces: Follow folder structure
- Files: PascalCase matching class names
- Folders: PascalCase for logical grouping

## Configuration Files
- `appsettings.json` / `appsettings.Development.json` - Application configuration
- `nuget.config` - NuGet package source configuration
- `*.http` files - HTTP request examples for testing

## Key Patterns
- Result pattern for error handling
- Middleware pipeline for cross-cutting concerns
- Extension methods for clean service registration
- Structured logging with correlation IDs
- Problem Details for API error responses