using CleanObservability.Core.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CleanObservability.Core.Setup;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseRequestContextEnrichment(this IApplicationBuilder app)
	{
		return app.UseMiddleware<CorrelationIdMiddleware>();
	}

	public static IApplicationBuilder UseExceptionToProblemDetails(this IApplicationBuilder app)
	{
		return app.UseMiddleware<ExceptionHandlingMiddleware>();
	}
}