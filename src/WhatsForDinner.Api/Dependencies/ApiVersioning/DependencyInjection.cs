namespace WhatsForDinner.Api.Dependencies.ApiVersioning;

internal static class DependencyInjection
{
	public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services) => services
		.AddEndpointsApiExplorer()
		.AddApiVersioning().AddApiExplorer()
		.Services;
}
