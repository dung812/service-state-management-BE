using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace ProductionManagement.API.Configurations
{
	public class ConfigMVCOption : IConfigureOptions<MvcOptions>
	{
		private IServiceProvider _serviceProvider;

		public ConfigMVCOption(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public void Configure(MvcOptions options)
		{
			//var formater = options.InputFormatters
			//.OfType<NewtonsoftJsonPatchInputFormatter>()
			//.First();

			//options.InputFormatters.Insert(0, formater);
			var a = options.InputFormatters.ToList();
		}
	}
}
