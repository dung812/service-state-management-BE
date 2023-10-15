using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductionManagement.API.Configurations.Swagger
{
	public class ConfigSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider _apiVersionProvider;

		public ConfigSwaggerOptions(IApiVersionDescriptionProvider apiVersionProvider)
			=> _apiVersionProvider = apiVersionProvider; //Inject the service to get all defined APIVersion

		public void Configure(SwaggerGenOptions options)
		{
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Enter token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "bearer"
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement {
				 {
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type=ReferenceType.SecurityScheme,
							Id="Bearer"
						}
					},
                    Array.Empty<string>()}
				}
			);

			foreach (var description in _apiVersionProvider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, new OpenApiInfo
				{
					Title = "ProductionManagement.API",
					Version = description.ApiVersion.ToString()
				});
			}
		}
	}
}
