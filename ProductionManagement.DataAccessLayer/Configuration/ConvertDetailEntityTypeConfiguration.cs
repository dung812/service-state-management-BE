using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Models;

namespace ProductionManagement.DataAccessLayer.Configuration
{
	internal class ConvertDetailEntityTypeConfiguration : IEntityTypeConfiguration<ConvertDetail>
	{
		public void Configure(EntityTypeBuilder<ConvertDetail> builder)
		{
			builder.HasKey(convert => new {convert.MaterialId, convert.UnitId});
			builder.HasOne(convert => convert.Unit).WithMany(unit => unit.ConvertDetails).HasForeignKey(convert => convert.UnitId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
