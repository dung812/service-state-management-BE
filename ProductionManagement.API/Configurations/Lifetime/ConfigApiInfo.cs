using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using ProductionManagement.DataContract.Common;

namespace ProductionManagement.API.Configurations.Lifetime
{
	public class ConfigApiInfo : IConfigureOptions<ApiConfigurations>
	{
		private readonly IApiVersionDescriptionProvider _apiVersionProvider;
		private readonly IConfiguration _configuration;
		public ConfigApiInfo(IApiVersionDescriptionProvider apiVersionProvider, IConfiguration configuration)
        {
			_apiVersionProvider = apiVersionProvider;
			_configuration = configuration;
		}
        public void Configure(ApiConfigurations options)
		{
			_configuration.GetSection("ApiConfiguration").Bind(options);
			options.AllowedApiVersions = _apiVersionProvider.ApiVersionDescriptions.Select(version => version.ApiVersion.ToString()).ToArray();
		}
	}
}
