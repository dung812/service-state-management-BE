using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ProductionManagement.API.Configurations.Auth.AuthorizationRequirement;

namespace ProductionManagement.API.Configurations.Auth
{
	public class CustomPolicyProvider : IAuthorizationPolicyProvider
	{
		public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
		public CustomPolicyProvider(IOptions<AuthorizationOptions> options)
		{
			FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
		}
		public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
		public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
		{
			var policyBuilder = new AuthorizationPolicyBuilder();
			policyBuilder.RequireAuthenticatedUser();
			policyBuilder.AddRequirements(new ConcurrencyStampRequirement());
			policyBuilder.AddRequirements(new RoleRequirement(policyName.Trim()));
			return Task.FromResult(policyBuilder.Build());
		}
		public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
		{
			return FallbackPolicyProvider.GetDefaultPolicyAsync();
		}
	}
}
