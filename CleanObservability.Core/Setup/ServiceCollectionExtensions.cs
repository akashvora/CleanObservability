using CleanObservability.Core.Diagnostics;
using CleanObservability.Core.Result;
using CleanObservability.Core.Results;
using CleanObservability.Core.Setup.Validation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
namespace CleanObservability.Core.Setup;


public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRequestContextEnrichment(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddTransient<CorrelationIdHandler>();
		return services;
	}

	public static IHttpClientBuilder AddObservabilityHttpClient(this IServiceCollection services, string name)
	{
		return services.AddHttpClient(name).AddHttpMessageHandler<CorrelationIdHandler>();
	}

	public static IServiceCollection AddObservability(this IServiceCollection services, Action<ObservabilityOptions> configure)
	{
		var options = new ObservabilityOptions();
		configure(options);

		services.AddSingleton(options);
		//services.AddSingleton<ProblemDetailsFactory, CustomValidationProblemDetailsFactoryOld>(); // ✅
		services.AddSingleton<ProblemDetailsFactory, CustomValidationProblemDetailsFactory>();
		services.AddSingleton<IErrorResponseBuilder, DefaultErrorResponseBuilder>();
		return services;
	}
}