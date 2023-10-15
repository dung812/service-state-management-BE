using Microsoft.AspNetCore.Identity;
using ProductionManagement.DataContract.Role;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Interfaces
{
	public interface IRoleService
	{
		Task<RoleViewContract> GetAsViewByNameAsync(string roleName);
		Task<RoleViewContract> CreateAsync(RoleContract roleContract);
		Task<IEnumerable<RoleViewContract>> GetAllAsync();
		Task<IdentityRole> GetByIdAsync(string roleId);
		Task<IdentityRole> GetByNameAsync(string roleName);
		Task<bool> DeleteAsync(IdentityRole role);
		Task<RoleViewContract> UpdateAsync(IdentityRole oldRole, string newName);
		Task<IEnumerable<Claim>> GetClaimsAsync(IdentityRole role, string? claimType = null);
		Task<bool> AddClaimAsync(IdentityRole role, Claim claim);
		Task<bool> RemoveClaimAsync(IdentityRole role, Claim claim);
		Task<IEnumerable<string>> GetRoleNamesForClaimAsync(Claim claim);
	}
}
