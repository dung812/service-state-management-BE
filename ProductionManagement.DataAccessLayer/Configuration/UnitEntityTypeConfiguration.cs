using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class UnitEntityTypeConfiguration : IEntityTypeConfiguration<Unit>
	{
		public void Configure(EntityTypeBuilder<Unit> builder)
		{
			builder.HasKey(unit => unit.Id);
			builder.Property(unit => unit.Name).HasMaxLength(128);
			builder.Property(unit => unit.Description).HasMaxLength(128);
		}
	}
}
