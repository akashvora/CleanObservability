using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanObservability.Core.Diagnostics;

public class CorrelationIdMiddleware
{
	private const string HeaderName = "X-Correlation-ID";
	private readonly RequestDelegate _next;

	public CorrelationIdMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
	{
		var correlationId = context.Request.Headers[HeaderName].FirstOrDefault() ?? Guid.NewGuid().ToString();

		using (logger.BeginScope(new Dictionary<string, object>
		{
			["CorrelationId"] = correlationId
		}))
		{
			context.Items[HeaderName] = correlationId;
			context.Response.Headers[HeaderName] = correlationId;

			await _next(context);
		}
	}
}