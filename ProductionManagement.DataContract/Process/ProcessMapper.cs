using AutoMapper;

namespace ProductionManagement.DataContract.Process
{
	internal class ProcessMapper : Profile
	{
		public ProcessMapper()
		{
			CreateMap<ProcessCreateContract, Models.Process>()
				.ForMember(process => process.Tasks, option => option.MapFrom(contract => contract.FlatGroup()));
			CreateMap<ProcessUpdateContract, Models.Process>();
			CreateMap<Models.Process, ProcessSimpleViewContract>()
				.ForMember(view => view.TotalTask, option => option.MapFrom(process => process.Tasks == null ? 0 : process.Tasks.Count));
			CreateMap<Models.Process, ProcessDetailViewContract>()
				.ForMember(view => view.TaskGroups, option => option.MapFrom(process => process.Tasks))
				.ForMember(view => view.CurrentStep, option => option.MapFrom(process => process.CurrentLevel));
		}
	}
}
