using System.ComponentModel;
using Enfo.Domain.Entities;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.Domain.Utils;
using JetBrains.Annotations;

namespace Enfo.Domain.Mapping
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
                LegalAuthorityId = resource.LegalAuthorityId ?? 0,
                OrderNumber = resource.OrderNumber,
                ProposedOrderPostedDate = resource.CreateAs == NewEnforcementOrderType.Proposed
                    ? resource.ProposedOrderPostedDate
                    : null,
                PublicationStatus = GetEntityPublicationState(resource.Progress),
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
            order.IsProposedOrder = resource.IsProposedOrder;
            order.IsExecutedOrder = resource.IsExecutedOrder;
            order.IsHearingScheduled = resource.IsHearingScheduled;
            order.LegalAuthorityId = resource.LegalAuthorityId;
            order.OrderNumber = resource.OrderNumber;
            order.ProposedOrderPostedDate = order.IsProposedOrder ? resource.ProposedOrderPostedDate : null;
            order.PublicationStatus = GetEntityPublicationState(resource.Progress);
            order.Requirements = resource.Requirements;
            order.SettlementAmount = resource.SettlementAmount;
        }

        public static EnforcementOrderUpdate ToEnforcementOrderUpdate(EnforcementOrderAdminView item) => new()
        {
            Cause = item.Cause,
            County = item.County,
            Requirements = item.Requirements,
            ExecutedDate = item.ExecutedDate,
            FacilityName = item.FacilityName,
            HearingDate = item.HearingDate,
            HearingLocation = item.HearingLocation,
            OrderNumber = item.OrderNumber,
            Progress = item.PublicationStatus,
            SettlementAmount = item.SettlementAmount,
            CommentContactId = item.CommentContact?.Id,
            IsInactiveCommentContact = !item.CommentContact?.Active ?? false,
            HearingContactId = item.HearingContact?.Id,
            IsInactiveHearingContact = !item.HearingContact?.Active ?? false,
            IsProposedOrder = item.IsProposedOrder,
            IsExecutedOrder = item.IsExecutedOrder,
            IsHearingScheduled = item.IsHearingScheduled,
            LegalAuthorityId = item.LegalAuthority.Id,
            CommentPeriodClosesDate = item.CommentPeriodClosesDate,
            ExecutedOrderPostedDate = item.ExecutedOrderPostedDate,
            ProposedOrderPostedDate = item.ProposedOrderPostedDate,
            HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate,
        };

        private static EnforcementOrder.PublicationState GetEntityPublicationState(PublicationState status) =>
            status switch
            {
                PublicationState.Draft => EnforcementOrder.PublicationState.Draft,
                PublicationState.Published => EnforcementOrder.PublicationState.Published,
                _ => throw new InvalidEnumArgumentException(nameof(status), (int) status, typeof(PublicationState))
            };
    }
}
