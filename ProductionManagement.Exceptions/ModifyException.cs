using Microsoft.AspNetCore.Http;

namespace ProductionManagement.Exceptions
{
	public class ModifyException : CustomException
	{
		public ModifyException(string message) : base(StatusCodes.Status400BadRequest, message) { }
	}
}
