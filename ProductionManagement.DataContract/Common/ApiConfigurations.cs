using System;

namespace ProductionManagement.DataContract.Common
{
	public class ApiConfigurations
	{
		public string? ApiUrl { get; set; }
		public string? MailDomain { get; set; }
        public string[] AllowedApiVersions { get; set; } = Array.Empty<string>();
	}
}
