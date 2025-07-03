using CleanObservability.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanObservability.Core.Extensions;

public static class ResultExtensions
{
	public static IActionResult ToActionResult(this CleanObservability.Core.Results.Result result, ControllerBase controller)
	{
		if (result.IsSuccess)
			return controller.NoContent();

		return new ObjectResult(result.Problem)
		{
			StatusCode = result.Problem?.Status ?? StatusCodes.Status500InternalServerError
		};
	}

	public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
	{
		if (result.IsSuccess && result.Value is not null)
			return controller.Ok(result.Value);

		return new ObjectResult(result.Problem)
		{
			StatusCode = result.Problem?.Status ?? StatusCodes.Status500InternalServerError
		};
	}

	public static IActionResult ToCreatedAtAction<T>(
		this Result<T> result,
		ControllerBase controller,
		string actionName,
		object routeValues,
		Func<T, object?> selectResource)
	{
		if (result.IsSuccess && result.Value is not null)
		{
			var resource = selectResource(result.Value);
			return controller.CreatedAtAction(actionName, routeValues, resource);
		}

		return new ObjectResult(result.Problem)
		{
			StatusCode = result.Problem?.Status ?? StatusCodes.Status500InternalServerError
		};
	}
}