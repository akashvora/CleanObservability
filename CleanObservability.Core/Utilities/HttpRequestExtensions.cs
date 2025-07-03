using Microsoft.AspNetCore.Http;

namespace CleanObservability.Demo.Utilities;

public static class HttpRequestExtensions
{
	public static bool IsApiRequest(this HttpRequest request)
	{
		return request.Headers.Accept.Any(h => h.Contains("application/json")) ||
			   request.ContentType?.Contains("application/json") == true;
	}
}