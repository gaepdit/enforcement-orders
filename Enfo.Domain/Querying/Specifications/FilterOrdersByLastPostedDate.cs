using Enfo.Domain.Entities;
using System;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByLastPostedDate : Specification<EnforcementOrder>
    {
        public FilterOrdersByLastPostedDate(DateTime lastPostedAfter)
        {
            ApplyCriteria(
                e => e.ExecutedDate > lastPostedAfter
                || e.ProposedOrderPostedDate > lastPostedAfter
            );
        }
    }
}
