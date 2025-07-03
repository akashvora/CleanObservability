using CleanObservability.Core.Errors;
using System.ComponentModel.DataAnnotations;

namespace Observability.Core.Results;

public static class ExceptionToApiProblem
{
	public static ApiProblem ToProblem(this Exception ex) =>
		ex switch
		{

			FileNotFoundException => new ApiProblem(404, "File not found", ApiErrorCodes.NotFound),
			UnauthorizedAccessException => new ApiProblem(401, "Unauthorized", ApiErrorCodes.Unauthorized),
			OperationCanceledException => new ApiProblem(408, "Request timed out", ApiErrorCodes.Timeout),
			ArgumentException => new ApiProblem(400, ex.Message, ApiErrorCodes.BadRequest),
			ApplicationException => new ApiProblem(422, ex.Message, ApiErrorCodes.Application),
			NotImplementedException => new ApiProblem(501, "Not implemented", ApiErrorCodes.Unimplemented),
			_ => new ApiProblem(500, "Unhandled exception", ApiErrorCodes.Unexpected)

		};
}


public record ApiProblem(int StatusCode, string Message, string Code);
