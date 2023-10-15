using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class ProcessTaskEntityTypeConfiguration : IEntityTypeConfiguration<ProcessTask>
	{
		public void Configure(EntityTypeBuilder<ProcessTask> builder)
		{
			builder.HasKey(task => task.Id);
			builder.Property(task => task.Name).HasMaxLength(128);
		}
	}
}
