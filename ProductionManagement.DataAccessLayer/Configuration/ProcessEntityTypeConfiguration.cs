using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class ProcessEntityTypeConfiguration : IEntityTypeConfiguration<Process>
	{
		public void Configure(EntityTypeBuilder<Process> builder)
		{
			builder.HasKey(process => process.Id);
			builder.Property(process => process.Name).HasMaxLength(128);

			builder.HasMany(process => process.Tasks).WithOne(task => task.Process).HasForeignKey(task => task.ProcessId);
		}
	}
}
