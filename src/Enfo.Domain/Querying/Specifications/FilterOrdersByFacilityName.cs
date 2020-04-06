using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByFacilityName : Specification<EnforcementOrder>
    {
        public FilterOrdersByFacilityName(string facilityFilter)
        {
            Guard.NotNullOrWhiteSpace(facilityFilter, nameof(facilityFilter));
            ApplyCriteria(e => e.FacilityName.ToLower().Contains(facilityFilter.ToLower()));
        }
    }
}
