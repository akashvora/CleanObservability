using CleanObservability.Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace CleanObservability.Core.Diagnostics;

public class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private const string TraceIdKey = "traceId";
	private const string CorrelationIdKey = "X-Correlation-ID";

	public ExceptionHandlingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			var traceId = context.TraceIdentifier;
			var correlationId = context.Items[CorrelationIdKey] as string ?? "unknown";

			var apiError = ApiErrorRegistry.From(ex);
			var problem = new ProblemDetails
			{
				Title = apiError.Title,
				Detail = apiError.Detail,
				Status = apiError.StatusCode,
				Type = $"https://httpstatuses.com/{apiError.StatusCode}",
				Extensions = new Dictionary<string, object?>() // Add Extensions property
			};

			problem.Extensions["code"] = apiError.Code;
			problem.Extensions["traceId"] = traceId;
			problem.Extensions["correlationId"] = correlationId;

			context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
			context.Response.ContentType = "application/problem+json";

			var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});

			await context.Response.WriteAsync(json);
		}
	}
}

// Define a derived class to add the Extensions property
//public class ExtendedProblemDetails : ProblemDetails
//{
//	public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
//}
public class ExtendedProblemDetails : ProblemDetails
{
	public ExtendedProblemDetails()
	{
		// Populate the inherited Extensions dictionary directly
		Extensions["traceId"] = Activity.Current?.Id;
		Extensions["requestId"] = Guid.NewGuid();
	}

	// You can still add new properties here
	public string? CustomSource { get; set; }
}
