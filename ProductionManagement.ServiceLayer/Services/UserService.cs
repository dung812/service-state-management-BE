using Microsoft.AspNetCore.Identity;
using ProductionManagement.DataContract.Common;
using ProductionManagement.DataContract.Constant;
using ProductionManagement.DataContract.Enum;
using ProductionManagement.DataContract.Mapper;
using ProductionManagement.DataContract.User;
using ProductionManagement.Exceptions;
using ProductionManagement.Models;
using ProductionManagement.ServiceLayer.Extensions;
using ProductionManagement.ServiceLayer.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		public UserService(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<UserViewContract> CreateAsync(User userToCreate, string[] roleNames, string? newPassword)
		{
			userToCreate.CreatedDate = DateTime.UtcNow;
			userToCreate.ModifiedDate = DateTime.UtcNow;

			var createResult = await _userManager.CreateAsync(userToCreate, newPassword ?? DefaultPassword.OtherRole);
			if (!createResult.Succeeded)
			{
				throw new ModifyException(createResult.ToErrorStrings());
			}

			var addToRoleResult = await _userManager.AddToRolesAsync(userToCreate, roleNames);
			if (!addToRoleResult.Succeeded)
			{
				throw new ModifyException(addToRoleResult.ToErrorStrings());
			}

			return AsView(userToCreate);
		}
		public PagedList<UserViewContract> GetAllWithPaging(UserQueryCriteria filter)
		{
			var userQuery = _userManager.Users;

			if (filter.Role != null)
			{
				userQuery = _userManager.GetUsersInRoleAsync(filter.Role).Result.AsQueryable();
			}

			if (!string.IsNullOrEmpty(filter.SearchString))
			{
				userQuery = userQuery.Where(user =>
					(user.Name.Replace(" ", "").ToLower().Contains(filter.SearchString.Replace(" ", "").ToLower())) ||
					(user.Email.Replace(" ", "").ToLower().Contains(filter.SearchString.Replace(" ", "").ToLower()))
				);
			}

			var pagedUser = userQuery.Paginate(filter.Page, filter.Limit);
			return pagedUser.ToPagedList<UserViewContract>();
		}

		public async Task<UserViewContract> GetAsViewByIdAsync(string userId)
		{
			var userFound = await GetByIdAsync(userId);
			var userView = AsView(userFound);
			userView.Roles = string.Join(Separator.CommaSeparator, await _userManager.GetRolesAsync(userFound));
			return userView;
		}

		public async Task<User> GetByIdAsync(string? userId)
		{
			return await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException($"User with the id - {userId} doesn't exist");
		}

		public async Task<bool> UpdateAsync(User userToUpdate)
		{
			userToUpdate.ModifiedDate = DateTime.UtcNow;

			var updateResult = await _userManager.UpdateAsync(userToUpdate);

			if (!updateResult.Succeeded)
			{
				throw new ModifyException(updateResult.ToErrorStrings());
			}

			return updateResult.Succeeded;
		}

		public async Task<bool> UpdateRoleAsync(User userToUpdate, string[] newRoleNames)
		{
			userToUpdate.ModifiedDate = DateTime.UtcNow;
			var allUserRoles = await _userManager.GetRolesAsync(userToUpdate);

			if (!newRoleNames.OrderBy(newRole => newRole).SequenceEqual(allUserRoles.OrderBy(currenRole => currenRole)))
			{
				var removeRoleResult = await _userManager.RemoveFromRolesAsync(userToUpdate, allUserRoles);
				if (!removeRoleResult.Succeeded)
				{
					throw new ModifyException(removeRoleResult.ToErrorStrings());
				}

				var addToRoleResult = await _userManager.AddToRolesAsync(userToUpdate, newRoleNames);
				if (!addToRoleResult.Succeeded)
				{
					throw new ModifyException(addToRoleResult.ToErrorStrings());
				}

				return addToRoleResult.Succeeded;
			}
			return true;
		}
		public async Task<bool> ChangePasswordAsync(User userToChangePassword, string newPassword, string? currentPassword = null)
		{
			if (currentPassword == null)
			{
				await _userManager.RemovePasswordAsync(userToChangePassword);
				var addPasswordResult = await _userManager.AddPasswordAsync(userToChangePassword, newPassword);
				return addPasswordResult.Succeeded;
			}
			else
			{
				if (newPassword.CompareTo(currentPassword) == 0)
				{
					throw new ModifyException("The new password must be different from the old one");
				}

				var changePasswordResult = await _userManager.ChangePasswordAsync(userToChangePassword, currentPassword, newPassword);
				if (!changePasswordResult.Succeeded)
				{
					throw new ModifyException(changePasswordResult.ToErrorStrings());
				}
				userToChangePassword.ModifiedDate = DateTime.UtcNow;
				await _userManager.UpdateAsync(userToChangePassword);
				return changePasswordResult.Succeeded;
			}
		}

		public async Task<User> GetByUserNameAsync(string userName)
		{
			return await _userManager.FindByNameAsync(userName) ?? throw new NotFoundException($"User with the username - {userName} doesn't exist");
		}

		public async Task<bool> DisableAsync(string userId)
		{
			var userToDisable = await GetByIdAsync(userId);
			userToDisable.ModifiedDate = DateTime.UtcNow;
			userToDisable.IsDisabled = !userToDisable.IsDisabled;
			var disabledResult = await _userManager.UpdateAsync(userToDisable);

			if (!disabledResult.Succeeded)
			{
				throw new ModifyException(disabledResult.ToErrorStrings());
			}

			return disabledResult.Succeeded;
		}

		private UserViewContract AsView(User user)
		{
			return Mapping.Mapper.Map<UserViewContract>(user);
		}
	}
}
