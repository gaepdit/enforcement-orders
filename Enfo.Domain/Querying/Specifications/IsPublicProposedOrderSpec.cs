using Enfo.Domain.Entities;
using System;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.Domain.Querying
{
    public class IsPublicProposedOrderSpec : Specification<EnforcementOrder>
    {
        public IsPublicProposedOrderSpec()
        {
            // e.IsPublicProposedOrder spelled out for Entity Framework
            ApplyCriteria(
                e => !e.Deleted
                && e.Active
                && e.PublicationStatus == PublicationState.Published
                && e.IsProposedOrder
                && e.ProposedOrderPostedDate.HasValue
                && e.ProposedOrderPostedDate.Value <= DateTime.Today
            );
        }
    }
}
