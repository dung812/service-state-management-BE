using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class MaterialEntityTypeConfiguration : IEntityTypeConfiguration<Material>
	{
		public void Configure(EntityTypeBuilder<Material> builder)
		{
			builder.HasKey(unit => unit.Id);
			builder.Property(unit => unit.Id).HasMaxLength(50);
			builder.Property(unit => unit.NameByDefault).HasMaxLength(128);
			builder.Property(unit => unit.NameByCustomer).HasMaxLength(128);
			builder.Property(unit => unit.NameByVender).HasMaxLength(128);

			builder.HasOne<Unit>(material => material.Unit).WithMany(unit => unit.Materials).HasForeignKey(material => material.UnitId).OnDelete(DeleteBehavior.NoAction);
			builder.HasMany<ConvertDetail>(material => material.ConvertDetails).WithOne(convert => convert.Material).HasForeignKey(convert => convert.MaterialId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne<Category>(material => material.Category).WithMany(category => category.Materials).HasForeignKey(material => material.CategoryId);
		}
	}
}
