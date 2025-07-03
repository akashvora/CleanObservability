// Result.cs
using Microsoft.AspNetCore.Mvc;

namespace CleanObservability.Core.Results;

public class Result
{
	public bool IsSuccess { get; }
	public ProblemDetails? Problem { get; }

	protected Result(bool isSuccess, ProblemDetails? problem)
	{
		IsSuccess = isSuccess;
		Problem = problem;
	}

	public static Result Success() => new(true, null);
	public static Result Failure(string title, string detail, int status) =>
		new(false, new ProblemDetails
		{
			Title = title,
			Detail = detail,
			Status = status,
			Type = $"https://httpstatuses.com/{status}"
		});
}
