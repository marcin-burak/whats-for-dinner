using Asp.Versioning.Builder;

namespace WhatsForDinner.Api.Dependencies.ApiVersioning;

internal static class ApiVersionSets
{
	public static ApiVersionSet Common { get; } = new ApiVersionSetBuilder(null)
		.HasApiVersion(new(1, 0))
		.ReportApiVersions()
		.Build();
}
