using Microsoft.Extensions.Options;
using WhatsForDinner.Api.HttpClient;
using WhatsForDinner.Blazor.Dependencies.Yarp;

namespace WhatsForDinner.Blazor.Dependencies.Api;

internal static class DependencyInjection
{
	public static IServiceCollection AddApiHttpClient(this IServiceCollection services) => services
		.AddTransient<ApiAuthenticationMiddleware>()
		.AddHttpClient<ApiClient>((serviceProvider, httpClient) =>
		{
			var yarpOptions = serviceProvider.GetRequiredService<IOptions<YarpOptions>>().Value;
			httpClient.BaseAddress = new(yarpOptions.ApiUrl);
		})
		.ConfigurePrimaryHttpMessageHandler(serviceProvider =>
		{
			var yarpOptions = serviceProvider.GetRequiredService<IOptions<YarpOptions>>().Value;

			if (yarpOptions.DisableApiSslCertificateValidation)
			{
				return new HttpClientHandler
				{
					ServerCertificateCustomValidationCallback = (_, _, _, _) => true
				};
			}

			return new HttpClientHandler();
		})
		.AddHttpMessageHandler<ApiAuthenticationMiddleware>()
		.Services;
}
