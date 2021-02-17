using Enfo.Domain.Entities;

namespace Enfo.Repository.Querying
{
    public class FilterByActiveItems<T> : Specification<T>
        where T : IActive
    {
        public FilterByActiveItems(bool includeInactive = false)
        {
            ApplyCriteria(e => includeInactive || e.Active);
        }
    }
}
