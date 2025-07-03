using CleanObservability.Core.Results;
using CleanObservability.Core.Utilities;
using CleanObservability.Demo.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace CleanObservability.Core.Setup.Validation;

public class CustomValidationProblemDetailsFactory : ProblemDetailsFactory
{
	private readonly IErrorResponseBuilder _errorBuilder;

	public CustomValidationProblemDetailsFactory(IErrorResponseBuilder errorBuilder)
	{
		_errorBuilder = errorBuilder;
	}

	public override ValidationProblemDetails CreateValidationProblemDetails(
	HttpContext httpContext,
	ModelStateDictionary modelStateDictionary,
	int? statusCode = null,
	string? title = null,
	string? type = null,
	string? detail = null,
	string? instance = null)
	{
		var traceId = httpContext.TraceIdentifier;

		var response = _errorBuilder.FromModelState(modelStateDictionary, traceId);

		// Convert ApiErrorResponse into ASP.NET’s ValidationProblemDetails structure
		var problemDetails = new ValidationProblemDetails(modelStateDictionary)
		{
			Status = StatusCodes.Status400BadRequest,
			Title = "Validation Failed",
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
			Detail = response.Message,
			Instance = httpContext.Request.Path
		};

		// Attach the traceId as an extension for observability
		problemDetails.Extensions["traceId"] = traceId;
		problemDetails.Extensions["errors"] = response.Errors;

		return problemDetails;
	}


	public override ProblemDetails CreateProblemDetails(
		HttpContext httpContext,
		int? statusCode = null,
		string? title = null,
		string? type = null,
		string? detail = null,
		string? instance = null)
	{
		return new ProblemDetails
		{
			Status = statusCode ?? 500,
			Title = title ?? "An error occurred.",
			Type = type ?? "https://tools.ietf.org/html/rfc7807",
			Detail = detail,
			Instance = instance,
		};
	}
}