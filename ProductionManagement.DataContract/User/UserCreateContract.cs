using ProductionManagement.DataContract.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.User
{
	public class UserCreateContract : CreateContract<Models.User>
	{
		public string? Id { get; set; }
		[MaxLength(255, ErrorMessage = "Name must be less than 255 characters long")]
		[Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
		public string Name { get; set; } = null!;
		[MaxLength(255, ErrorMessage = "Email must be less than 255 characters long")]
		[Required(ErrorMessage = "Email is required", AllowEmptyStrings = false)]
		[EmailAddress(ErrorMessage = "Email is invalid")]
		public string Email { get; set; } = null!;
		[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^\._-])(?!.*\s).{8,}$", ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character")]
		public string? NewPassword { get; set; }
		[Required(ErrorMessage = "Role is required", AllowEmptyStrings = false)]
		public string Roles { get; set; } = null!;

		public override Models.User ToNewEntity()
		{
			var user = new Models.User
			{
				Name = Name,
				UserName = Email[..Email.IndexOf('@')],
				Email = Email,
				IsDisabled = false,
				CreatedDate = DateTime.UtcNow,
				ModifiedDate = DateTime.UtcNow
			};
			return user;
		}
	}
}
