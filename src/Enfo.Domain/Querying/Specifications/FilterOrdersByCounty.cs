using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByCounty : Specification<EnforcementOrder>
    {
        public FilterOrdersByCounty(string county)
        {
            Guard.NotNullOrWhiteSpace(county, nameof(county));
            ApplyCriteria(e => e.County.ToLower().Equals(county.ToLower()));
        }
    }
}
