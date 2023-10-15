using AutoMapper;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductionManagement.DataContract.Migration
{
	internal class MigrationMapper : Profile
	{
		public MigrationMapper()
		{
			CreateMap<HistoryRow, MigrationViewContract>().ReverseMap();
		}
	}
}
