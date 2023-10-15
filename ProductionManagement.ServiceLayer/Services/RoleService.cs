using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.DataContract.Mapper;
using ProductionManagement.DataContract.Role;
using ProductionManagement.Exceptions;
using ProductionManagement.RepositoryLayer.Interfaces;
using ProductionManagement.ServiceLayer.Extensions;
using ProductionManagement.ServiceLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Services
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IRoleClaimRepository _roleClaimRepository;

		public RoleService(RoleManager<IdentityRole> roleManager, IRoleClaimRepository roleClaimRepository)
		{
			_roleManager = roleManager;
			_roleClaimRepository = roleClaimRepository;
		}

		public async Task<bool> AddClaimAsync(IdentityRole role, Claim claim)
		{
			var roleClaims = await _roleManager.GetClaimsAsync(role);
			var existedClaim = roleClaims.FirstOrDefault(roleClaim => roleClaim.Type == claim.Type && roleClaim.Value == claim.Value);
			if (existedClaim == null)
			{
				var addResult = await _roleManager.AddClaimAsync(role, claim);
				return addResult.Succeeded ? addResult.Succeeded : throw new ModifyException(addResult.ToErrorStrings());
			}
			return true;
		}

		public async Task<RoleViewContract> CreateAsync(RoleContract roleContract)
		{
			var roleToAdd = roleContract.ToNewRole();
			var isRoleExisted = await _roleManager.RoleExistsAsync(roleToAdd.Name);
			if (isRoleExisted)
			{
				throw new PrimaryKeyConstraintException("Role is existed");
			}

			var result = await _roleManager.CreateAsync(roleToAdd);
			if (!result.Succeeded)
			{
				throw new ModifyException(result.ToErrorStrings());
			}
			return AsView(roleToAdd);
		}

		public async Task<bool> DeleteAsync(IdentityRole role)
		{
			var result = await _roleManager.DeleteAsync(role);
			if (!result.Succeeded)
			{
				throw new ModifyException(result.ToErrorStrings());
			}
			return result.Succeeded;
		}

		public async Task<IEnumerable<RoleViewContract>> GetAllAsync()
		{
			return AsListView(await _roleManager.Roles.ToListAsync());
		}

		public async Task<IdentityRole> GetByIdAsync(string roleId)
		{
			return await _roleManager.FindByIdAsync(roleId) ?? throw new NotFoundException($"Role with Id - {roleId} doesn't exist");
		}

		public async Task<RoleViewContract> GetAsViewByNameAsync(string roleName)
		{
			return AsView(await GetByNameAsync(roleName));
		}

		public async Task<IdentityRole> GetByNameAsync(string roleName)
		{
			return await _roleManager.FindByNameAsync(roleName) ?? throw new NotFoundException($"Role {roleName} doesn't exist");
		}

		public async Task<IEnumerable<Claim>> GetClaimsAsync(IdentityRole role, string? claimType = null)
		{
			var roleClaims = await _roleManager.GetClaimsAsync(role);
			if (claimType != null)
			{
				roleClaims = roleClaims.Where(claim => claim.Type == claimType).ToList();
			}
			return roleClaims;
		}

		public async Task<IEnumerable<string>> GetRoleNamesForClaimAsync(Claim claim)
		{
			var roleClaims = await _roleClaimRepository.Entities
				.Join(_roleManager.Roles, claim => claim.RoleId, role => role.Id, (claims, roles) => new { claims, roles })
				.Where(roleClaim =>
					roleClaim.claims.ClaimType == claim.Type &&
					roleClaim.claims.ClaimValue == claim.Value)
				.ToListAsync();
			return roleClaims.Select(roleClaim => roleClaim.roles.Name);
		}

		public async Task<bool> RemoveClaimAsync(IdentityRole role, Claim claim)
		{
			var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
			if (!removeResult.Succeeded)
			{
				throw new ModifyException(removeResult.ToErrorStrings());
			}
			return removeResult.Succeeded;
		}

		public async Task<RoleViewContract> UpdateAsync(IdentityRole oldRole, string newName)
		{
			oldRole.Name = newName;
			var result = await _roleManager.UpdateAsync(oldRole);

			if (!result.Succeeded)
			{
				throw new ModifyException(result.ToErrorStrings());
			}

			return AsView(oldRole);
		}

		private RoleViewContract AsView(IdentityRole role)
		{
			return Mapping.Mapper.Map<RoleViewContract>(role);
		}

		private IEnumerable<RoleViewContract> AsListView(IEnumerable<IdentityRole> role)
		{
			return Mapping.Mapper.Map<IEnumerable<RoleViewContract>>(role);
		}
	}
}
