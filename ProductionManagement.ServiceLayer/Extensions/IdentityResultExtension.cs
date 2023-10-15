using Microsoft.AspNetCore.Identity;
using ProductionManagement.DataContract.Constant;
using System.Linq;

namespace ProductionManagement.ServiceLayer.Extensions
{
	public static class IdentityResultExtension
	{
		public static string ToErrorStrings(this IdentityResult identityError, char separator = Separator.BlankSeparator)
		{
			return string.Join(separator, identityError.Errors.Select(error => error.Description));
		}
	}
}
