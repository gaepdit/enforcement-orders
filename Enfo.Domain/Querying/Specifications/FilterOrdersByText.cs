using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByText : Specification<EnforcementOrder>
    {
        public FilterOrdersByText(string textContains)
        {
            Check.NotNullOrWhiteSpace(textContains, nameof(textContains));
            ApplyCriteria(e => e.Cause.ToLower().Contains(textContains.ToLower())
                || e.Requirements.ToLower().Contains(textContains.ToLower()));
        }
    }
}
