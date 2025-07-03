using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace CleanObservability.Demo.Middlewares;

public class SerilogRequestEnricherMiddleware
{
	private readonly RequestDelegate _next;

	public SerilogRequestEnricherMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		var traceId = context.TraceIdentifier;
		var requestPath = context.Request.Path;

		using (LogContext.PushProperty("TraceId", traceId))
		using (LogContext.PushProperty("RequestPath", requestPath))
		{
			await _next(context);
		}
	}
}