using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ProductionManagement.DataAccessLayer.CurrentUser
{

	public class CurrentUserModel : ICurrentUserModel
	{
		private readonly IHttpContextAccessor _contextAccessor;
		public CurrentUserModel(IHttpContextAccessor httpContextAccessor)
		{
			_contextAccessor = httpContextAccessor;
		}
		public string Username
		{
			get
			{
				var nameClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name);
				if (nameClaim != null)
				{
					return nameClaim.Value;
				}
				return string.Empty;
			}
		}
	}
}
