using Microsoft.AspNetCore.Identity;
using ProductionManagement.DataContract.Mapper;
using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.Role
{
	public class RoleContract
	{
		[Required(ErrorMessage = "Role name is required", AllowEmptyStrings = false)]
		public string Name { get; set; } = null!;

		public IdentityRole ToNewRole()
		{
			return Mapping.Mapper.Map<IdentityRole>(this);
		}
	}
}
