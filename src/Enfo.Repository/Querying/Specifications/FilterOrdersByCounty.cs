using Enfo.Domain.Entities;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Querying
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
