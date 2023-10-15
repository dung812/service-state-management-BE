using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ProductionManagement.API.Configurations.Auth.AuthorizationRequirement;

namespace ProductionManagement.API.Configurations.Auth.RequirementHandler
{
	public static class ConfigAuthorization
	{
		/// <summary>
		/// Add authorization with policy
		/// </summary>
		/// <param name="services">IServiceCollection</param>
		public static void AddAuthorizationWithPolicy(this IServiceCollection services)
		{
			services.AddAuthorization(option =>
			{
				option.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
					.RequireAuthenticatedUser()
					.AddRequirements(new ConcurrencyStampRequirement())
					.Build();
			});

			services.AddTransient<IAuthorizationPolicyProvider, CustomPolicyProvider>();
			services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();
			services.AddScoped<IAuthorizationHandler, ConcurrencyStampRequirementHandler>();
		}
	}
}
