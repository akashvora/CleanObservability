using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

public class CustomValidationProblemDetailsFactoryOld : ProblemDetailsFactory
{
	private readonly ApiBehaviorOptions _options;

	public CustomValidationProblemDetailsFactoryOld(IOptions<ApiBehaviorOptions> options)
	{
		_options = options.Value;
	}

	public override ProblemDetails CreateProblemDetails(HttpContext httpContext,
		int? statusCode = null,
		string? title = null,
		string? type = null,
		string? detail = null,
		string? instance = null)
	{
		return new ProblemDetails
		{
			Title = title ?? "Unexpected error",
			Status = statusCode ?? StatusCodes.Status500InternalServerError,
			Type = type ?? "error"
		};
	}

	public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext,
		ModelStateDictionary modelStateDictionary,
		int? statusCode = null,
		string? title = null,
		string? type = null,
		string? detail = null,
		string? instance = null)
	{
		var traceId = httpContext?.TraceIdentifier;

		var problemDetails = new ValidationProblemDetails(modelStateDictionary)
		{
			Title = title ?? "validation_error",
			Status = statusCode ?? StatusCodes.Status400BadRequest,
			Type = type ?? "https://yourdomain.com/docs/errors/validation_error",
			Instance = instance ?? httpContext?.Request.Path
		};

		problemDetails.Extensions["traceId"] = httpContext?.TraceIdentifier;

		return problemDetails;

	}
}
