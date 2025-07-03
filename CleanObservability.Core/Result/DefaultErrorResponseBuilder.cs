using CleanObservability.Core.Errors;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CleanObservability.Core.Results;

namespace CleanObservability.Core.Result
{
	public class DefaultErrorResponseBuilder : IErrorResponseBuilder
	{
		public ApiErrorResponse FromModelState(ModelStateDictionary modelState, string traceId) =>
			new()
			{
				Code = "VALIDATION_ERROR",
				Message = "One or more validation errors occurred.",
				TraceId = traceId,
				Errors = modelState
					.Where(x => x.Value?.Errors.Count > 0)
					.ToDictionary(
						x => x.Key,
						x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
					)
			};

		public ApiErrorResponse FromException(Exception ex, string traceId) =>
			new()
			{
				Code = "UNEXPECTED_ERROR",
				Message = ex.Message,
				TraceId = traceId
			};
	}

}
