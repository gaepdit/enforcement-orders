using Enfo.Domain.Entities;
using System;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.Domain.Querying
{
    public class IsPublicExecutedOrderSpec : Specification<EnforcementOrder>
    {
        public IsPublicExecutedOrderSpec()
        {
            // e.IsPublicExecutedOrder spelled out for Entity Framework
            ApplyCriteria(
                e => !e.Deleted
                && e.Active
                && e.PublicationStatus == PublicationState.Published
                && e.IsExecutedOrder
                && e.ExecutedOrderPostedDate.HasValue
                && e.ExecutedOrderPostedDate.Value <= DateTime.Today
            );
        }
    }
}
