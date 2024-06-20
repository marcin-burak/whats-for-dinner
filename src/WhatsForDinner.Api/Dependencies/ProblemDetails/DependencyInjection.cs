namespace WhatsForDinner.Api.Dependencies.ProblemDetails;

internal static class DependencyInjection
{
	public static IServiceCollection AddProblemDetailsConfiguration(this IServiceCollection services) => services
		.AddProblemDetails();
}
