using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanObservability.Core.Errors
{
	public record ApiErrorResponse
	{
		public string Code { get; init; } = "UNEXPECTED_ERROR";
		public string Message { get; init; } = "An unexpected error occurred.";
		public string? TraceId { get; init; }
		public IDictionary<string, string[]>? Errors { get; init; }
	}

}
