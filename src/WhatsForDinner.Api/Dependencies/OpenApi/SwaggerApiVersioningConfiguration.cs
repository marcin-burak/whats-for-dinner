using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WhatsForDinner.Api.Dependencies.OpenApi;

internal sealed class SwaggerApiVersioningConfiguration(IApiVersionDescriptionProvider apiVersioningDescriptionProvider) : IConfigureOptions<SwaggerGenOptions>
{
	private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider = apiVersioningDescriptionProvider;

	public void Configure(SwaggerGenOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);

		foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
		{
			options.SwaggerDoc(
				description.GroupName,
				new OpenApiInfo
				{
					Title = "What's for dinner API",
					Version = description.ApiVersion.ToString()
				}
			);
		}
	}
}
