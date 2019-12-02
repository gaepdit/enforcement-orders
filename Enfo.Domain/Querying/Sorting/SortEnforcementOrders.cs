using Enfo.Domain.Entities;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.Domain.Querying
{
    public class SortEnforcementOrders : Sorting<EnforcementOrder>
    {
        public SortEnforcementOrders(EnforcementOrderSorting EnforcementOrderSorting)
        {
            switch (EnforcementOrderSorting)
            {
                default:
                case EnforcementOrderSorting.FacilityAsc:
                    ApplyOrderBy(e => e.FacilityName.Trim().Trim(new char[] { '\n', '\r', '\t' }));
                    break;
                case EnforcementOrderSorting.FacilityDesc:
                    ApplyOrderByDescending(e => e.FacilityName.Trim().Trim(new char[] { '\n', '\r', '\t' }));
                    break;
                case EnforcementOrderSorting.DateAsc:
                    ApplyOrderBy(e => e.LastPostedDate);
                    break;
                case EnforcementOrderSorting.DateDesc:
                    ApplyOrderByDescending(e => e.LastPostedDate);
                    break;
            }
        }
    }
}
