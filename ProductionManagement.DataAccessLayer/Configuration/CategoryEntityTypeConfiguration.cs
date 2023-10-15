using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasKey(category => category.Id);
			builder.Property(category => category.Id).HasMaxLength(50);
			builder.Property(category => category.Name).HasMaxLength(128);

			builder.HasMany<Category>(category => category.ChildCategories).WithOne(category => category.ParentCategory).HasForeignKey(category => category.ParentCategoryId);
		}
	}
}
