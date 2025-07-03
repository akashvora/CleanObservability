using Microsoft.AspNetCore.Mvc;
using CleanObservability.Core.Errors;
using BaseResult = CleanObservability.Core.Results.Result;

public class Result<T> : BaseResult //Result.Result
{
	public T? Value { get; }

	private Result(bool isSuccess, T? value, ProblemDetails? problem)
		: base(isSuccess, problem)
	{
		Value = value;
	}

	public static Result<T> Success(T value)
		=> new(true, value, null);

	public static new Result<T> Failure(string title, string detail, int status)
		=> new(false, default, new ProblemDetails
		{
			Title = title,
			Detail = detail,
			Status = status,
			Type = $"https://httpstatuses.com/{status}"
		});

	public static Result<T> Failure(ApiError error)
		=> new(false, default, new ProblemDetails
		{
			Title = error.Title,
			Detail = error.Detail,
			Status = error.StatusCode,
			Type = $"https://httpstatuses.com/{error.StatusCode}",
			Extensions = { ["code"] = error.Code }
		});
}
