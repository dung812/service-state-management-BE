using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class WarehouseEntityTypeConfiguration : IEntityTypeConfiguration<Warehouse>
	{
		public void Configure(EntityTypeBuilder<Warehouse> builder)
		{
			builder.HasKey(workshop => workshop.Id);
			builder.Property(workshop => workshop.Id).HasMaxLength(50);
			builder.Property(workshop => workshop.Name).HasMaxLength(128);
			builder.Property(workshop => workshop.PhoneNumber).HasMaxLength(15);
			builder.Property(workshop => workshop.Email).HasMaxLength(128);
		}
	}
}
