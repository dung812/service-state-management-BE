using ProductionManagement.DataContract.Mapper;

namespace ProductionManagement.DataContract.Base
{
	public abstract class UpdateContract<TEntity>
	{
		public virtual TEntity UpdateFor(TEntity entity) {
			return Mapping.Mapper.Map(this, entity);
		}
	}
}
