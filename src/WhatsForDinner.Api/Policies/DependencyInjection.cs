namespace WhatsForDinner.Api.Policies;

internal static class DependencyInjection
{
	public static IServiceCollection AddPolicies(this IServiceCollection services) => services
		.AddScoped<CommonGroupPolicy>();
}
