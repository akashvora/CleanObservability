using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CleanObservability.Core.Diagnostics;

public class CorrelationIdHandler : DelegatingHandler
{
	private const string HeaderName = "X-Correlation-ID";
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var context = _httpContextAccessor.HttpContext;
		if (context?.Items[HeaderName] is string correlationId &&
			!string.IsNullOrWhiteSpace(correlationId))
		{
			request.Headers.TryAddWithoutValidation(HeaderName, correlationId);
		}

		return base.SendAsync(request, cancellationToken);
	}
}