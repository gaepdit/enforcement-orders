using Enfo.Domain.Entities;

namespace Enfo.Domain.Specifications
{
    public class ExcludeInactiveItemsSpecification<T> : Specification<T>
        where T : BaseEntity
    {
        public ExcludeInactiveItemsSpecification() : base(e => e.Active) { }
    }
}
