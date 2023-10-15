using ProductionManagement.Exceptions;
using System.Security.Claims;

namespace ProductionManagement.API.Extensions
{
	public static class HttpContextExtension
	{
		public static string GetAuthenticatedUsername(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Name) ?? throw new RestrictedPermissionException("You are not authenticated or don't have permission");
		}

		public static string GetAuthenticatedRoles(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Role) ?? throw new RestrictedPermissionException("You are not authenticated or don't have permission");
		}

		public static string? GetClaim(this ClaimsPrincipal user, string claimType)
		{
			return user.FindFirst(claimType)?.Value;
		}
	}
}
