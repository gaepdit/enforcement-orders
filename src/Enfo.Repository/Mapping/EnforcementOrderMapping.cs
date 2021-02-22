using System;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Utils;
using static Enfo.Repository.Resources.EnforcementOrder.EnforcementOrderCreate;

namespace Enfo.Repository.Mapping
{
    public static class EnforcementOrderMapping
    {
        public static EnforcementOrderView ToEnforcementOrderView(this EnforcementOrder item)
        {
            Guard.NotNull(item, nameof(item));

            return new EnforcementOrderView()
            {
                Id = item.Id,
                FacilityName = item.FacilityName,
                County = item.County,
                LegalAuthority = item.LegalAuthority.ToLegalAuthorityView(),
                Cause = item.Cause,
                Requirements = item.Requirements,
                SettlementAmount = item.SettlementAmount,
                OrderNumber = item.OrderNumber,
                IsPublicProposedOrder = item.GetIsPublicProposedOrder(),
                IsProposedOrder = item.GetIsPublicProposedOrder() && item.IsProposedOrder,
                CommentPeriodClosesDate = item.GetIsPublicProposedOrder() ? item.CommentPeriodClosesDate : null,
                CommentContact = item.CommentContactId.HasValue &&
                    item.GetIsPublicProposedOrder() &&
                    item.CommentPeriodClosesDate.HasValue &&
                    item.CommentPeriodClosesDate >= DateTime.Today
                        ? item.CommentContact.ToEpdContactView()
                        : null,
                ProposedOrderPostedDate = item.GetIsPublicProposedOrder() ? item.ProposedOrderPostedDate : null,
                IsPublicExecutedOrder = item.GetIsPublicExecutedOrder(),
                IsExecutedOrder = item.GetIsPublicExecutedOrder() && item.IsExecutedOrder,
                ExecutedDate = item.GetIsPublicExecutedOrder() ? item.ExecutedDate : null,
                IsHearingScheduled = item.IsHearingScheduled,
                HearingDate = item.HearingDate,
                HearingLocation = item.HearingLocation,
                HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate,
                HearingContact = item.HearingContactId.HasValue &&
                    item.HearingCommentPeriodClosesDate.HasValue &&
                    item.HearingCommentPeriodClosesDate >= DateTime.Today
                        ? item.HearingContact.ToEpdContactView()
                        : null,
            };
        }

        public static EnforcementOrderDetailedView ToEnforcementOrderDetailedView(this EnforcementOrder item)
        {
            Guard.NotNull(item, nameof(item));

            return new EnforcementOrderDetailedView()
            {
                Id = item.Id,
                FacilityName = item.FacilityName,
                County = item.County,
                LegalAuthority = item.LegalAuthority.ToLegalAuthorityView(),
                Cause = item.Cause,
                Requirements = item.Requirements,
                SettlementAmount = item.SettlementAmount,
                Deleted = item.Deleted,
                PublicationStatus = item.PublicationStatus,
                OrderNumber = item.OrderNumber,
                LastPostedDate = item.GetLastPostedDate(),
                IsProposedOrder = item.IsProposedOrder,
                CommentPeriodClosesDate = item.CommentPeriodClosesDate,
                CommentContact = item.CommentContactId.HasValue ? item.CommentContact.ToEpdContactView() : null,
                ProposedOrderPostedDate = item.ProposedOrderPostedDate,
                IsExecutedOrder = item.IsExecutedOrder,
                ExecutedDate = item.ExecutedDate,
                IsHearingScheduled = item.IsHearingScheduled,
                HearingDate = item.HearingDate,
                HearingLocation = item.HearingLocation,
                HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate,
                HearingContact = item.HearingContactId.HasValue ? item.HearingContact.ToEpdContactView() : null,
            };
        }

        public static EnforcementOrderSummaryView ToEnforcementOrderSummaryView(this EnforcementOrder item)
        {
            Guard.NotNull(item, nameof(item));

            return new EnforcementOrderSummaryView()
            {
                Id = item.Id,
                FacilityName = item.FacilityName,
                County = item.County,
                LegalAuthority = item.LegalAuthority.ToLegalAuthorityView(),
                OrderNumber = item.OrderNumber,
                IsPublicProposedOrder = item.GetIsPublicProposedOrder(),
                IsProposedOrder = item.GetIsPublicProposedOrder() && item.IsProposedOrder,
                CommentPeriodClosesDate = item.GetIsPublicProposedOrder() ? item.CommentPeriodClosesDate : null,
                ProposedOrderPostedDate = item.GetIsPublicProposedOrder() ? item.ProposedOrderPostedDate : null,
                IsPublicExecutedOrder = item.GetIsPublicExecutedOrder(),
                IsExecutedOrder = item.GetIsPublicExecutedOrder() && item.IsExecutedOrder,
                ExecutedDate = item.GetIsPublicExecutedOrder() ? item.ExecutedDate : null,
            };
        }

        public static EnforcementOrder ToEnforcementOrder(this EnforcementOrderCreate resource)
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
                PublicationStatus = resource.PublicationStatus,
                Requirements = resource.Requirements,
                SettlementAmount = resource.SettlementAmount
            };
        }

        public static void UpdateFrom(this EnforcementOrder order, EnforcementOrderUpdate resource)
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
            order.PublicationStatus = resource.PublicationStatus;
            order.Requirements = resource.Requirements;
            order.SettlementAmount = resource.SettlementAmount;
        }
    }
}