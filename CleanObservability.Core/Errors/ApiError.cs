namespace CleanObservability.Core.Errors
{
	public class ApiError
	{
		public string Code { get; init; } = default!;
		public string Title { get; init; } = default!;
		public string Detail { get; init; } = default!;
		public int StatusCode { get; init; }
	}
}