using System.ComponentModel;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Utils;
using JetBrains.Annotations;
using static Enfo.Repository.Resources.EnforcementOrder.EnforcementOrderCreate;

namespace Enfo.Repository.Mapping
{
    public static class EnforcementOrderMapping
    {
        public static EnforcementOrder ToEnforcementOrder([NotNull] this EnforcementOrderCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));

            return new EnforcementOrder
            {
                Cause = resource.Cause,
                CommentContactId = resource.CreateAs == NewEnforcementOrderType.Proposed
                    ? resource.CommentContactId
                    : null,
                CommentPeriodClosesDate = resource.CreateAs == NewEnforcementOrderType.Proposed
                    ? resource.CommentPeriodClosesDate
                    : null,
                County = resource.County,
                ExecutedDate = resource.CreateAs == NewEnforcementOrderType.Executed ? resource.ExecutedDate : null,
                ExecutedOrderPostedDate = resource.CreateAs == NewEnforcementOrderType.Executed
                    ? resource.ExecutedOrderPostedDate
                    : null,
                FacilityName = resource.FacilityName,
                HearingCommentPeriodClosesDate =
                    resource.IsHearingScheduled ? resource.HearingCommentPeriodClosesDate : null,
                HearingContactId = resource.IsHearingScheduled ? resource.HearingContactId : null,
                HearingDate = resource.IsHearingScheduled ? resource.HearingDate : null,
                HearingLocation = resource.IsHearingScheduled ? resource.HearingLocation : null,
                IsExecutedOrder = resource.CreateAs == NewEnforcementOrderType.Executed,
                IsHearingScheduled = resource.IsHearingScheduled,
                IsProposedOrder = resource.CreateAs == NewEnforcementOrderType.Proposed,
                LegalAuthorityId = resource.LegalAuthorityId,
                OrderNumber = resource.OrderNumber,
                ProposedOrderPostedDate = resource.CreateAs == NewEnforcementOrderType.Proposed
                    ? resource.ProposedOrderPostedDate
                    : null,
                PublicationStatus = GetEntityPublicationState(resource.PublicationStatus),
                Requirements = resource.Requirements,
                SettlementAmount = resource.SettlementAmount
            };
        }

        public static void UpdateFrom([NotNull] this EnforcementOrder order, [NotNull] EnforcementOrderUpdate resource)
        {
            Guard.NotNull(order, nameof(order));
            Guard.NotNull(resource, nameof(resource));

            order.Cause = resource.Cause;
            order.CommentContactId = order.IsProposedOrder ? resource.CommentContactId : null;
            order.CommentPeriodClosesDate = order.IsProposedOrder ? resource.HearingCommentPeriodClosesDate : null;
            order.County = resource.County;
            order.ExecutedDate = resource.IsExecutedOrder ? resource.ExecutedDate : null;
            order.ExecutedOrderPostedDate = resource.IsExecutedOrder ? resource.ExecutedOrderPostedDate : null;
            order.FacilityName = resource.FacilityName;
            order.HearingCommentPeriodClosesDate =
                resource.IsHearingScheduled ? resource.HearingCommentPeriodClosesDate : null;
            order.HearingContactId = resource.IsHearingScheduled ? resource.HearingContactId : null;
            order.HearingDate = resource.IsHearingScheduled ? resource.HearingDate : null;
            order.HearingLocation = resource.IsHearingScheduled ? resource.HearingLocation : null;
            order.IsExecutedOrder = resource.IsExecutedOrder;
            order.IsHearingScheduled = resource.IsHearingScheduled;
            order.LegalAuthorityId = resource.LegalAuthorityId;
            order.OrderNumber = resource.OrderNumber;
            order.ProposedOrderPostedDate = order.IsProposedOrder ? resource.ProposedOrderPostedDate : null;
            order.PublicationStatus = GetEntityPublicationState(resource.PublicationStatus);
            order.Requirements = resource.Requirements;
            order.SettlementAmount = resource.SettlementAmount;
        }

        private static EnforcementOrder.PublicationState GetEntityPublicationState(PublicationState status) =>
            status switch
            {
                PublicationState.Draft => EnforcementOrder.PublicationState.Draft,
                PublicationState.Published => EnforcementOrder.PublicationState.Published,
                _ => throw new InvalidEnumArgumentException(nameof(status), (int) status, typeof(PublicationState))
            };
    }
}