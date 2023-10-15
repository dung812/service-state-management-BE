using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class AuditEntityTypeConfiguration : IEntityTypeConfiguration<Audit>
	{
		public void Configure(EntityTypeBuilder<Audit> builder)
		{
		}
	}
}
