using Enfo.Domain.Entities;
using System;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByExecutedOrderPostedDate : Specification<EnforcementOrder>
    {
        public FilterOrdersByExecutedOrderPostedDate(DateTime fromDate, DateTime tillDate)
        {
            ApplyCriteria(
                e => e.ExecutedOrderPostedDate >= fromDate
                && e.ExecutedOrderPostedDate <= tillDate
            );
        }
    }
}
