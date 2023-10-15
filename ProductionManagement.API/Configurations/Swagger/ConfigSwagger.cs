using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ProductionManagement.API.Configurations.Swagger
{
    public static class ConfigSwagger
    {
        public static void AddSwaggerWithVersioning(this IServiceCollection services)
        {
            services.AddSwaggerGen(); //Generate json file
            services.ConfigureOptions<ConfigSwaggerOptions>(); //Configure for SwaggerGen
        }

        public static void UseSwaggerUiWithVersioning(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                var apiProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in apiProvider.ApiVersionDescriptions)
                {
                    //Create Swagger endpoint for each APIVersion
                    option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                };
            });
        }
    }
}
