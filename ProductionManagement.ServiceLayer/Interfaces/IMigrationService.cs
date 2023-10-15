using ProductionManagement.DataContract.Migration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Interfaces
{
	public interface IMigrationService
	{
		Task<IEnumerable<MigrationViewContract>> GetAllAppliedMigrationsAsync();
	}
}
