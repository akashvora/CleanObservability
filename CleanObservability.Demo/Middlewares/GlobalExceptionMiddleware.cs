using System.Diagnostics;
using System.Text.Json;
using Observability.Core.Results;

namespace CleanObservability.WebAPI.Middleware;

public class GlobalExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<GlobalExceptionMiddleware> _logger;

	public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context); // pass to next middleware/controller
		}
		catch (Exception ex)
		{
			var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
			var problem = ex.ToProblem(); // map using core logic

			_logger.LogError(ex, "Unhandled exception caught: {Message}", ex.Message);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = problem.StatusCode;

			var response = new
			{
				traceId,
				status = problem.StatusCode,
				title = problem.Message,
				code = problem.Code, // ← use your ApiErrorCodes here
				detail = ex.Message
			};

			await context.Response.WriteAsync(JsonSerializer.Serialize(response));
		}
	}
}