using Microsoft.Extensions.Logging;

namespace CleanObservability.Core.Setup;

public static class ObservabilityBuilderExtensions
{
	public static ILoggerFactory EnrichWithObservability(this ILoggerFactory loggerFactory, ObservabilityOptions options)
	{
		// You could add OpenTelemetry or Serilog enrichers here in the future
		return loggerFactory;
	}
}