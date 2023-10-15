using Microsoft.AspNetCore.Identity;
using ProductionManagement.DataAccessLayer.Context;
using ProductionManagement.Models;

namespace ProductionManagement.API.Configurations.Auth
{
    public static class ConfigIdentity
	{
		public static void AddCustomIdentity(this IServiceCollection services)
		{
			services.AddIdentity<User, IdentityRole>(option =>
			{
				option.User.RequireUniqueEmail = true;
				option.Password.RequiredLength = 8;
			})
				.AddEntityFrameworkStores<ProductionManagementContext>()
				.AddDefaultTokenProviders();
		}
	}
}
