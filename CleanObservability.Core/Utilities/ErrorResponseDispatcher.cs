using CleanObservability.Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanObservability.Core.Utilities
{
	public static class ErrorResponseDispatcher
	{
		public static IActionResult Dispatch(HttpContext httpContext, ApiErrorResponse error)
		{
			var isApiRequest =
				httpContext.Request.Headers.Accept.Any(h => h.Contains("application/json")) ||
				httpContext.Request.Path.StartsWithSegments("/api");

			if (isApiRequest)
			{
				var problem = new ValidationProblemDetails
				{
					Title = error.Message,
					Status = StatusCodes.Status400BadRequest,
					Detail = error.Code,
					Extensions =
				{
					["traceId"] = error.TraceId,
					["errors"] = error.Errors
				}
				};

				return new ObjectResult(problem)
				{
					StatusCode = problem.Status,
					ContentTypes = { "application/problem+json" }
				};
			}

			return new ViewResult
			{
				ViewName = "Error",
				ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ApiErrorResponse>(
					new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
					new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()
				)
				{
					Model = error
				}
			};
		}
	}
}