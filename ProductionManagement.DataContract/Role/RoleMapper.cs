using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ProductionManagement.DataContract.Role
{
	internal class RoleMapper : Profile
	{
		public RoleMapper()
		{
			CreateMap<RoleContract, IdentityRole>().ReverseMap();
			CreateMap<IdentityRole, RoleViewContract>();
		}
	}
}
