using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ProductionManagement.API.Configurations.Versioning
{
    public static class ConfigVersioning
    {
        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.AssumeDefaultVersionWhenUnspecified = true; //if client not specify the api version, use default version
                option.ReportApiVersions = true; //write "api-supported-version" or "api-deprecated-version" to header
                option.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(), //Read version from url path
                    new QueryStringApiVersionReader("version"), //A key in query string to read version
                    new HeaderApiVersionReader("Accept-version") //A key in header to read version
                );
            });

            //Explore the API and find all APIVersion defined
            services.AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'ProductionManagement_api-v'VVV"; //Format these APIVersion like 'ProductionManagement_api-v1.0.2', 'ProductionManagement_api-v2.0.1'
				option.SubstituteApiVersionInUrl = true; //Replace 'version:ApiVersion' in route template with actual version value.
            });
        }
    }
}
