using Enfo.Domain.Entities;

namespace Enfo.Domain.Specifications
{
    public class ExcludeInactiveItemsSpec<TEntity> : BaseSpecification<TEntity> where TEntity : BaseEntity
    {
        public ExcludeInactiveItemsSpec(bool includeInactive = false)
            : base(e => e.Active || includeInactive) { }
    }
}
