using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class WorkshopEntityTypeConfiguration : IEntityTypeConfiguration<Workshop>
	{
		public void Configure(EntityTypeBuilder<Workshop> builder)
		{
			builder.HasKey(workshop => workshop.Id);
			builder.Property(workshop => workshop.Name).HasMaxLength(128);
			builder.Property(workshop => workshop.PhoneNumber).HasMaxLength(15);
			builder.Property(workshop => workshop.Address).HasMaxLength(128);
		}
	}
}
