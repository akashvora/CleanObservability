namespace CleanObservability.Core.Errors;

public static class ApiErrorCodes
{
	public const string NotFound = "not_found";
	public const string Conflict = "conflict_error";
	public const string Validation = "validation_error";
	public const string Unauthorized = "unauthorized";
	public const string Forbidden = "forbidden";
	public const string Timeout = "timeout";
	public const string Unimplemented = "not_implemented";
	public const string Unexpected = "unexpected_error";
	public const string Application = "application_error";
	public const string BadRequest = "bad_request";

}