using AutoMapper;

namespace ProductionManagement.DataContract.User
{
	public class UserMapper : Profile
	{
		public UserMapper()
		{
			CreateMap<Models.User, UserViewContract>();
			CreateMap<UserUpdateContract, Models.User>();
		}
	}
}
