using Microsoft.AspNetCore.Http;

namespace ProductionManagement.Exceptions
{
	public class RestrictedPermissionException : CustomException
	{
		public RestrictedPermissionException(string message) : base(StatusCodes.Status403Forbidden, message)
		{
		}
	}
}
