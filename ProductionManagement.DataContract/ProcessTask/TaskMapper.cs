using AutoMapper;
using ProductionManagement.DataContract.Enum;

namespace ProductionManagement.DataContract.Process
{
	internal class TaskMapper : Profile
	{
		public TaskMapper()
		{
			CreateMap<TaskCreateContract, Models.ProcessTask>()
				.ForMember(task => task.Status, option => option.MapFrom(task => task.Level == 1 ? (int)ProcessTaskStatus.Processing : (int)ProcessTaskStatus.Waiting));
			CreateMap<Models.ProcessTask, TaskSimpleViewContract>()
				.ForMember(view => view.ViewStatus, option => option.MapFrom(task => ((ProcessTaskStatus)task.Status).ToString()));
			CreateMap<TaskCreateContract, TaskSimpleViewContract>();
		}
	}
}
