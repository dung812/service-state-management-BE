using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductionManagement.API.Configurations;
using ProductionManagement.API.Configurations.Auth;
using ProductionManagement.API.Configurations.Auth.RequirementHandler;
using ProductionManagement.API.Configurations.Filter;
using ProductionManagement.API.Configurations.Lifetime;
using ProductionManagement.API.Configurations.Middleware;
using ProductionManagement.API.Configurations.Swagger;
using ProductionManagement.API.Configurations.Versioning;
using ProductionManagement.API.Extensions;
using ProductionManagement.DataAccessLayer.Context;
using ProductionManagement.DataAccessLayer.Data;
using ProductionManagement.DataContract.Common;
using ProductionManagement.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.AddAppConfigurations();
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllerWithCustomFilter();
builder.Services.AddCustomApiVersioning();
builder.Services.AddSwaggerWithVersioning();
builder.Services.AddHttpContextAccessor();

//builder.Services.ConfigureOptions<ConfigMVCOption>();

builder.Services.AddEntityRepositories();
builder.Services.AddCustomIdentity();
builder.Services.AddCurrentUser();

builder.Services.AddAuthenticationWithBearer(configuration);
builder.Services.AddAuthorizationWithPolicy();

builder.Services.ConfigureOptions<ConfigApiInfo>();

builder.Services.AddDbContext<ProductionManagementContext>(option =>
{
	option.UseSqlServer(
		configuration.GetConnectionString("local_ProductionManagement")
	);
});

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "AllowAll", policy => {
		policy.AllowAnyOrigin();
		policy.AllowAnyMethod();
		policy.AllowAnyHeader();
	});
});

WebApplication app = builder.Build();

app.UseGlobalExceptionHandler();

// Seed user identity
using (IServiceScope scope = app.Services.CreateScope())
{
	IServiceProvider serviceProvider = scope.ServiceProvider;
	try
	{
		ProductionManagementContext context = serviceProvider.GetRequiredService<ProductionManagementContext>();

		UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
		RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		IOptions<ApiConfigurations> apiConfigurations = serviceProvider.GetRequiredService<IOptions<ApiConfigurations>>();

		await context.Database.MigrateAsync();
		await DbSeeder.SeedUserAsync(userManager, roleManager, apiConfigurations.Value.MailDomain);
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex);
	}
}

if (!app.Environment.IsProduction())
{
	app.UseSwaggerUiWithVersioning();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
