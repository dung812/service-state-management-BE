using Microsoft.AspNetCore.Identity;
using ProductionManagement.DataAccessLayer.Context;
using ProductionManagement.RepositoryLayer.Interfaces;
using System;

namespace ProductionManagement.RepositoryLayer.Repositories
{
	public class RoleClaimRepository : BaseRepository<IdentityRoleClaim<string>, Guid>, IRoleClaimRepository
	{
		public RoleClaimRepository(ProductionManagementContext dbContext) : base(dbContext)
		{
		}
	}
}
