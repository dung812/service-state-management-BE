using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProductionManagement.API.Configurations.Auth
{
	public static class ConfigAuthentication
	{
		public static void AddAuthenticationWithBearer(this IServiceCollection services, IConfiguration configuration)
		{
			_ = services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = configuration.GetValue<string>("JWT:ValidAudience"),
					ValidIssuer = configuration.GetValue<string>("JWT:ValidIssuer"),
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Secret"))),
					ClockSkew = TimeSpan.Zero, 
				};
			});
		}
	}
}
