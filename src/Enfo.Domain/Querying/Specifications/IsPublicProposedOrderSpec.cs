﻿using System;
using Enfo.Domain.Entities;
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
                && e.PublicationStatus == PublicationState.Published
                && e.IsProposedOrder
                && e.ProposedOrderPostedDate.HasValue
                && e.ProposedOrderPostedDate.Value <= DateTime.Today
            );
        }
    }
}
