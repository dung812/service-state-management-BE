using ProductionManagement.DataContract.Mapper;

namespace ProductionManagement.DataContract.Base
{
	public abstract class CreateContract<TEntity>
	{
		public virtual TEntity ToNewEntity()
		{
			return Mapping.Mapper.Map<TEntity>(this);
		}
	}
}
