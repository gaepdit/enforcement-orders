using Enfo.Domain.Entities;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Querying
{
    public class FilterOrdersByText : Specification<EnforcementOrder>
    {
        public FilterOrdersByText(string textContains)
        {
            Guard.NotNullOrWhiteSpace(textContains, nameof(textContains));
            ApplyCriteria(e => e.Cause.ToLower().Contains(textContains.ToLower())
                || e.Requirements.ToLower().Contains(textContains.ToLower()));
        }
    }
}
