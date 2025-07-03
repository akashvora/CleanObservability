using Microsoft.AspNetCore.Http;

namespace CleanObservability.Core.Errors;

public static class ApiErrorRegistry
{
	public static ApiError From(Exception ex) =>
		ex switch
		{
			ArgumentNullException or ArgumentException => new ApiError
			{
				Code = ApiErrorCodes.Validation,
				Title = "Invalid Argument",
				Detail = ex.Message,
				StatusCode = StatusCodes.Status400BadRequest
			},

			UnauthorizedAccessException => new ApiError
			{
				Code = ApiErrorCodes.Unauthorized,
				Title = "Unauthorized",
				Detail = "You are not authorized to perform this action.",
				StatusCode = StatusCodes.Status401Unauthorized
			},

			InvalidOperationException => new ApiError
			{
				Code = ApiErrorCodes.Conflict,
				Title = "Conflict Detected",
				Detail = ex.Message,
				StatusCode = StatusCodes.Status409Conflict
			},

			_ => new ApiError
			{
				Code = ApiErrorCodes.Unexpected,
				Title = "Unexpected Error",
				Detail = ex.Message,
				StatusCode = StatusCodes.Status500InternalServerError
			}
		};
}