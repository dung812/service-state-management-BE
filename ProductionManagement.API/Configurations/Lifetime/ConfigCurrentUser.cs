using ProductionManagement.DataAccessLayer.CurrentUser;

namespace ProductionManagement.API.Configurations.Lifetime
{
	public static class ConfigCurrentUser
	{
		public static void AddCurrentUser(this IServiceCollection services)
		{
			services.AddScoped<ICurrentUserModel, CurrentUserModel>();
		}
	}
}
