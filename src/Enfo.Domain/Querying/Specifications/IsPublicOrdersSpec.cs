﻿using System;
using Enfo.Domain.Entities;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.Domain.Querying
{
    public class IsPublicOrdersSpec : Specification<EnforcementOrder>
    {
        public IsPublicOrdersSpec()
        {
            // e.IsPublic spelled out for Entity Framework
            ApplyCriteria(e =>
                !e.Deleted
                && e.PublicationStatus == PublicationState.Published
                && ((e.IsExecutedOrder && e.ExecutedOrderPostedDate.HasValue && e.ExecutedOrderPostedDate.Value <= DateTime.Today)
                || (e.IsProposedOrder && e.ProposedOrderPostedDate.HasValue && e.ProposedOrderPostedDate.Value <= DateTime.Today)));
        }
    }
}
