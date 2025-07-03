using CleanObservability.Core.Errors;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanObservability.Core.Results
{
	public interface IErrorResponseBuilder
	{
			ApiErrorResponse FromModelState(ModelStateDictionary modelState, string traceId);
			ApiErrorResponse FromException(Exception exception, string traceId);

	}
}
