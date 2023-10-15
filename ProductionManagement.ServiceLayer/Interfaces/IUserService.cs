using ProductionManagement.DataContract.Common;
using ProductionManagement.DataContract.Enum;
using ProductionManagement.DataContract.User;
using ProductionManagement.Models;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Interfaces
{
	public interface IUserService
	{
		Task<UserViewContract> GetAsViewByIdAsync(string userId);
		Task<User> GetByIdAsync(string? userId);
		Task<User> GetByUserNameAsync(string userName);
		PagedList<UserViewContract> GetAllWithPaging(UserQueryCriteria filter);
		Task<UserViewContract> CreateAsync(User userToCreate, string[] roleNames, string? newPassword);
		Task<bool> UpdateAsync(User newUser);
		Task<bool> UpdateRoleAsync(User userToUpdate, string[] newRoleNames);
		Task<bool> DisableAsync(string userId);
		Task<bool> ChangePasswordAsync(User userToChangePassword, string newPassword, string? currentPassword = null);
	}
}
