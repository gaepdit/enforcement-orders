using Enfo.Domain.Entities;
using System;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByEndDate : Specification<EnforcementOrder>
    {
        public FilterOrdersByEndDate(DateTime tillDate, ActivityStatus status)
        {
            switch (status)
            {
                case ActivityStatus.Proposed:
                    ApplyCriteria(e => e.ProposedOrderPostedDate <= tillDate);
                    break;

                case ActivityStatus.Executed:
                    ApplyCriteria(e => e.ExecutedDate <= tillDate);
                    break;

                case ActivityStatus.All:
                    ApplyCriteria(e => e.ProposedOrderPostedDate <= tillDate || e.ExecutedDate <= tillDate);
                    break;
            }
        }
    }
}
