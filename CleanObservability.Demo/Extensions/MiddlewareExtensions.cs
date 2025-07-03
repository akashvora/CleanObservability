using CleanObservability.Demo.Middlewares;
using CleanObservability.WebAPI.Middleware;

namespace CleanObservability.Demo.Extensions
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseObservabilityMiddlewares(this IApplicationBuilder app)
		{
			return app
				.UseMiddleware<SerilogRequestEnricherMiddleware>()
				.UseMiddleware<GlobalExceptionMiddleware>();
		}
	}
}
