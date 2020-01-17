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
                case EnforcementOrderSorting.FacilityAsc:
                    ApplyOrderBy(e => e.FacilityName.Trim().Trim(new char[] { '\n', '\r', '\t' }));
                    ApplyOrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate);
                    break;

                case EnforcementOrderSorting.FacilityDesc:
                    ApplyOrderByDescending(e => e.FacilityName.Trim().Trim(new char[] { '\n', '\r', '\t' }));
                    ApplyOrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate);
                    break;

                case EnforcementOrderSorting.DateAsc:
                    ApplyOrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate);
                    ApplyOrderBy(e => e.FacilityName.Trim().Trim(new char[] { '\n', '\r', '\t' }));
                    break;

                case EnforcementOrderSorting.DateDesc:
                    ApplyOrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate);
                    ApplyOrderBy(e => e.FacilityName.Trim().Trim(new char[] { '\n', '\r', '\t' }));
                    break;
            }
        }
    }
}
