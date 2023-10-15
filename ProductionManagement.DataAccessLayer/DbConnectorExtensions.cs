using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;
using System.Reflection;

namespace ProductionManagement.DataAccessLayer
{
    public static class DbConnectorExtensions
    {
        public static void ApplyDefaultEntityTypeConfiguration(this ModelBuilder builder)
        {
            var configTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract
                        && !t.IsGenericTypeDefinition
                        && t.GetTypeInfo().ImplementedInterfaces.Any(i => i.GetTypeInfo().IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                 );

            var entityMethod = typeof(ModelBuilder).GetMethods()
                .Single(x => x.Name == "Entity" &&
                x.IsGenericMethod &&
                x.ReturnType.IsGenericType &&
                x.ReturnType.GetGenericTypeDefinition() == typeof(EntityTypeBuilder<>));

            foreach (var type in configTypes)
            {
                var genericTypeArg = type.GetInterfaces().Single().GenericTypeArguments.Single();
                var genericMethod = entityMethod.MakeGenericMethod(genericTypeArg);
                var entityBuilder = genericMethod.Invoke(builder, null);
                var mapper = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
                mapper.GetType().GetMethod("Configure")?.Invoke(mapper, new[] { entityBuilder });
            }
        }
    }
}
