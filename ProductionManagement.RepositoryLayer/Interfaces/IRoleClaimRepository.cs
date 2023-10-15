using Microsoft.AspNetCore.Identity;
using System;

namespace ProductionManagement.RepositoryLayer.Interfaces
{
	public interface IRoleClaimRepository : IBaseRepository<IdentityRoleClaim<string>, Guid>
	{
	}
}
