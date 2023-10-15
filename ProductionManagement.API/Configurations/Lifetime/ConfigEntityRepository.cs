namespace ProductionManagement.API.Configurations.Lifetime
{
	public static class ConfigEntityRepository
	{
		public static void AddEntityRepositories(this IServiceCollection services)
		{
			services.Scan(scan => scan
			   .FromApplicationDependencies(assemblly => assemblly.GetName().FullName.Contains("RepositoryLayer"))
				   .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
				   .AsMatchingInterface()
				   .WithScopedLifetime()
			   .FromApplicationDependencies(assemblly => assemblly.GetName().FullName.Contains("ServiceLayer"))
				   .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
				   .AsMatchingInterface()
				   .WithScopedLifetime()
			);
		}
	}
}
