using Microsoft.AspNetCore.Authorization;
using ProductionManagement.API.Configurations.Auth.AuthorizationRequirement;
using ProductionManagement.API.Extensions;
using System.Security.Claims;

namespace ProductionManagement.API.Configurations.Auth.RequirementHandler
{
	public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
	{
		public RoleRequirementHandler() : base()
		{ }
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
		{
			var contextUser = context.User;
			var roleClaim = contextUser.Claims.Where(claim => claim.Type == ClaimTypes.Role).FirstOrDefault();

			if (roleClaim == null || contextUser.Identity == null)
			{
				context.Fail();
				return Task.CompletedTask;
			}

			var userRoles = roleClaim.Value.SplitAndRemoveBlank();
			if (requirement.Roles.SplitAndRemoveBlank().Any(role => userRoles.Contains(role.Trim(), StringComparer.OrdinalIgnoreCase)))
			{
				context.Succeed(requirement);
				return Task.CompletedTask;
			}

			context.Fail();
			return Task.CompletedTask;
		}
	}
}
