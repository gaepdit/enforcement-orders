using Enfo.Domain.Entities;
using System;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByStartDate : Specification<EnforcementOrder>
    {
        public FilterOrdersByStartDate(DateTime fromDate, ActivityStatus status)
        {
            switch (status)
            {
                case ActivityStatus.Proposed:
                    ApplyCriteria(e => e.ProposedOrderPostedDate >= fromDate);
                    break;

                case ActivityStatus.Executed:
                    ApplyCriteria(e => e.ExecutedDate >= fromDate);
                    break;

                case ActivityStatus.All:
                    ApplyCriteria(e => e.ProposedOrderPostedDate >= fromDate || e.ExecutedDate >= fromDate);
                    break;

                default:
                    ApplyCriteria(e => true);
                    break;
            }
        }
    }
}
