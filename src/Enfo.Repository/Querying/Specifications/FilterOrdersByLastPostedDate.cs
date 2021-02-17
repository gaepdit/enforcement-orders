using System;
using Enfo.Domain.Entities;

namespace Enfo.Repository.Querying
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
