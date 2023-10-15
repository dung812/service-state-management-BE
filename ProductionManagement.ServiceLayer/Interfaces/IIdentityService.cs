using ProductionManagement.DataContract.Identity;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Interfaces
{
	public interface IIdentityService
	{
		Task<LoginResponseContract?> AuthorizeAsync(LoginContract loginModel);
	}
}
