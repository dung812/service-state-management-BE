using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductionManagement.DataAccessLayer.Context;
using ProductionManagement.DataContract.Mapper;
using ProductionManagement.DataContract.Migration;
using ProductionManagement.ServiceLayer.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Services
{
	public class MigrationService : IMigrationService
	{
		private readonly ProductionManagementContext _context;
		public MigrationService(ProductionManagementContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<MigrationViewContract>> GetAllAppliedMigrationsAsync()
		{
			var appliedMigrations = await _context.Database.GetService<IHistoryRepository>().GetAppliedMigrationsAsync();
			return Mapping.Mapper.Map<IEnumerable<MigrationViewContract>>(appliedMigrations);
		}
	}
}
