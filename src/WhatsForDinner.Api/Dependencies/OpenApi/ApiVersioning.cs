using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace WhatsForDinner.Api.Dependencies.OpenApi;

internal class SwaggerDefaultValues : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var apiDescription = context.ApiDescription;

		operation.Deprecated |= apiDescription.IsDeprecated();

		foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
		{
			var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
			var response = operation.Responses[responseKey];

			foreach (var contentType in response.Content.Keys)
			{
				if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
				{
					response.Content.Remove(contentType);
				}
			}
		}

		if (operation.Parameters == null)
		{
			return;
		}

		foreach (var parameter in operation.Parameters)
		{
			var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

			parameter.Description ??= description.ModelMetadata?.Description;

			if (parameter.Schema.Default == null &&
				 description.DefaultValue != null &&
				 description.DefaultValue is not DBNull &&
				 description.ModelMetadata is ModelMetadata modelMetadata)
			{
				var json = JsonSerializer.Serialize(description.DefaultValue, modelMetadata.ModelType);
				parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
			}

			parameter.Required |= description.IsRequired;
		}
	}
}

internal sealed class ApiVersioning(IApiVersionDescriptionProvider apiVersioningDescriptionProvider) : IConfigureOptions<SwaggerGenOptions>
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
