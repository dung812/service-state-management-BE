using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProductionManagement.API.Configurations.Auth.AuthorizationRequirement;
using ProductionManagement.Models;
using ProductionManagement.ServiceLayer.Constants;

namespace ProductionManagement.API.Configurations.Auth.RequirementHandler
{
	public class ConcurrencyStampRequirementHandler : AuthorizationHandler<ConcurrencyStampRequirement>
	{
		private readonly UserManager<User> _userManager;

		public ConcurrencyStampRequirementHandler(UserManager<User> userManager) : base()
		{
			_userManager = userManager;
		}
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ConcurrencyStampRequirement requirement)
		{
			var contextUser = context.User;
			var concurrencyClaim = context.User.Claims.Where(claim => claim.Type == CustomClaimTypes.ConcurrencyStamp).FirstOrDefault();

			if (concurrencyClaim == null || contextUser.Identity == null)
			{
				context.Fail();
				return Task.CompletedTask;
			}

			var user = _userManager.FindByNameAsync(contextUser.Identity.Name).Result;
			if (user.ConcurrencyStamp == concurrencyClaim.Value)
			{
				context.Succeed(requirement);
				return Task.CompletedTask;
			}

			context.Fail();
			return Task.CompletedTask;
		}
	}
}
