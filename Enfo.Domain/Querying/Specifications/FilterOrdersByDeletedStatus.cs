using Enfo.Domain.Entities;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByDeletedStatus : Specification<EnforcementOrder>
    {
        public FilterOrdersByDeletedStatus(bool showDeleted = false)
        {
            // Either deleted or active items are returned; not both.
            ApplyCriteria(e => e.Deleted == showDeleted);
        }
    }
}
