using Enfo.Domain.Entities;

namespace Enfo.Domain.Querying
{
    public class ActiveItemsSpec<T> : Specification<T>
        where T : IActive
    {
        public ActiveItemsSpec(bool includeInactive = false)
        {
            ApplyCriteria(e => e.Active || includeInactive);
        }
    }
}
