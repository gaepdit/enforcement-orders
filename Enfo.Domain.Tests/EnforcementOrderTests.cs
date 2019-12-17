using Enfo.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.Domain.Tests
{
    public class EnforcementOrderTests
    {
        private readonly EnforcementOrder _order = new EnforcementOrder
        {
            Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
            CommentContactId = 2004,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            Deleted = false,
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "A diam maecenas",
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2004,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "venenatis urna cursus eget nunc scelerisque viverra mauris in aliquam sem",
            IsExecutedOrder = true,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 7,
            OrderNumber = "EPD-ACQ-7936",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            PublicationStatus = PublicationState.Published,
            Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
            SettlementAmount = 2000
        };

        [Fact]
        public void CreateExecutedOrderSucceeds()
        {
            var createOrderResult = CreateNewEnforcementOrderEntity(
                NewEnforcementOrderType.Executed, _order.Cause, _order.CommentContactId, _order.CommentPeriodClosesDate,
                _order.County, _order.FacilityName, _order.ExecutedDate, _order.ExecutedOrderPostedDate,
                _order.HearingCommentPeriodClosesDate, _order.HearingContactId, _order.HearingDate,
                _order.HearingLocation, _order.IsHearingScheduled, _order.LegalAuthorityId, _order.OrderNumber,
                _order.ProposedOrderPostedDate, _order.PublicationStatus, _order.Requirements, _order.SettlementAmount);

            var expectedOrder = new EnforcementOrder
            {
                Cause = _order.Cause,
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = _order.County,
                Deleted = _order.Deleted,
                ExecutedDate = _order.ExecutedDate,
                ExecutedOrderPostedDate = _order.ExecutedOrderPostedDate,
                FacilityName = _order.FacilityName,
                HearingCommentPeriodClosesDate = _order.HearingCommentPeriodClosesDate,
                HearingContactId = _order.HearingContactId,
                HearingDate = _order.HearingDate,
                HearingLocation = _order.HearingLocation,
                IsExecutedOrder = _order.IsExecutedOrder,
                IsHearingScheduled = _order.IsHearingScheduled,
                IsProposedOrder = false,
                LegalAuthorityId = _order.LegalAuthorityId,
                OrderNumber = _order.OrderNumber,
                ProposedOrderPostedDate = null,
                PublicationStatus = _order.PublicationStatus,
                Requirements = _order.Requirements,
                SettlementAmount = _order.SettlementAmount
            };

            createOrderResult.Success.Should().BeTrue();
            createOrderResult.NewItem.Should().BeEquivalentTo(expectedOrder);
            createOrderResult.NewItem.GetLastPostedDate().Should().Be(_order.ExecutedDate);
            createOrderResult.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void CreateProposedOrderWithHearingSucceeds()
        {
            var createOrderResult = CreateNewEnforcementOrderEntity(
                NewEnforcementOrderType.Proposed, _order.Cause, _order.CommentContactId, _order.CommentPeriodClosesDate,
                _order.County, _order.FacilityName, _order.ExecutedDate, _order.ExecutedOrderPostedDate,
                _order.HearingCommentPeriodClosesDate, _order.HearingContactId, _order.HearingDate,
                _order.HearingLocation, _order.IsHearingScheduled, _order.LegalAuthorityId, _order.OrderNumber,
                _order.ProposedOrderPostedDate, _order.PublicationStatus, _order.Requirements, _order.SettlementAmount);

            var expectedOrder = new EnforcementOrder
            {
                Cause = _order.Cause,
                CommentContactId = _order.CommentContactId,
                CommentPeriodClosesDate = _order.CommentPeriodClosesDate,
                County = _order.County,
                Deleted = _order.Deleted,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = _order.FacilityName,
                HearingCommentPeriodClosesDate = _order.HearingCommentPeriodClosesDate,
                HearingContactId = _order.HearingContactId,
                HearingDate = _order.HearingDate,
                HearingLocation = _order.HearingLocation,
                IsExecutedOrder = false,
                IsHearingScheduled = _order.IsHearingScheduled,
                IsProposedOrder = _order.IsProposedOrder,
                LegalAuthorityId = _order.LegalAuthorityId,
                OrderNumber = _order.OrderNumber,
                ProposedOrderPostedDate = _order.ProposedOrderPostedDate,
                PublicationStatus = _order.PublicationStatus,
                Requirements = _order.Requirements,
                SettlementAmount = _order.SettlementAmount,
            };

            createOrderResult.Success.Should().BeTrue();
            createOrderResult.NewItem.Should().BeEquivalentTo(expectedOrder);
            createOrderResult.NewItem.GetLastPostedDate().Should().Be(_order.ProposedOrderPostedDate);
            createOrderResult.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void CreateProposedOrderWithNoHearingSucceeds()
        {
            var createOrderResult = CreateNewEnforcementOrderEntity(
                NewEnforcementOrderType.Proposed,
                _order.Cause,
                _order.CommentContactId,
                _order.CommentPeriodClosesDate,
                _order.County,
                _order.FacilityName,
                _order.ExecutedDate,
                _order.ExecutedOrderPostedDate,
                _order.HearingCommentPeriodClosesDate,
                _order.HearingContactId,
                _order.HearingDate,
                _order.HearingLocation,
                false,
                _order.LegalAuthorityId,
                _order.OrderNumber,
                _order.ProposedOrderPostedDate,
                _order.PublicationStatus,
                _order.Requirements,
                _order.SettlementAmount);

            var expectedOrder = new EnforcementOrder
            {
                Cause = _order.Cause,
                CommentContactId = _order.CommentContactId,
                CommentPeriodClosesDate = _order.CommentPeriodClosesDate,
                County = _order.County,
                Deleted = _order.Deleted,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = _order.FacilityName,
                HearingCommentPeriodClosesDate = null,
                HearingContactId = null,
                HearingDate = null,
                HearingLocation = null,
                IsExecutedOrder = false,
                IsHearingScheduled = false,
                IsProposedOrder = _order.IsProposedOrder,
                LegalAuthorityId = _order.LegalAuthorityId,
                OrderNumber = _order.OrderNumber,
                ProposedOrderPostedDate = _order.ProposedOrderPostedDate,
                PublicationStatus = _order.PublicationStatus,
                Requirements = _order.Requirements,
                SettlementAmount = _order.SettlementAmount,
            };

            createOrderResult.Success.Should().BeTrue();
            createOrderResult.NewItem.Should().BeEquivalentTo(expectedOrder);
            createOrderResult.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void CreateProposedOrderWithMissingDataFails()
        {
            var createOrderResult = CreateNewEnforcementOrderEntity(
                NewEnforcementOrderType.Proposed,
                _order.Cause,
                null,
                null,
                _order.County,
                _order.FacilityName,
                _order.ExecutedDate,
                _order.ExecutedOrderPostedDate,
                _order.HearingCommentPeriodClosesDate,
                _order.HearingContactId,
                _order.HearingDate,
                _order.HearingLocation,
                _order.IsHearingScheduled,
                _order.LegalAuthorityId,
                _order.OrderNumber,
                null,
                _order.PublicationStatus,
                _order.Requirements,
                _order.SettlementAmount);

            createOrderResult.Success.Should().BeFalse();
            createOrderResult.NewItem.Should().BeNull();
            createOrderResult.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(3)
                .And.ContainKeys(new string[] { "CommentContact", "CommentPeriodClosesDate", "ProposedOrderPostedDate" });
        }

        [Fact]
        public void CreateExecutedOrderWithMissingDataFails()
        {
            var createOrderResult = CreateNewEnforcementOrderEntity(
                NewEnforcementOrderType.Executed,
                _order.Cause,
                _order.CommentContactId,
                _order.CommentPeriodClosesDate,
                _order.County,
                _order.FacilityName,
                null,
                null,
                _order.HearingCommentPeriodClosesDate,
                _order.HearingContactId,
                _order.HearingDate,
                _order.HearingLocation,
                _order.IsHearingScheduled,
                _order.LegalAuthorityId,
                _order.OrderNumber,
                _order.ProposedOrderPostedDate,
                _order.PublicationStatus,
                _order.Requirements,
                _order.SettlementAmount);

            createOrderResult.Success.Should().BeFalse();
            createOrderResult.NewItem.Should().BeNull();
            createOrderResult.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.ContainKeys(new string[] { "ExecutedDate", "ExecutedOrderPostedDate" });
        }

        [Fact]
        public void CreateOrderWithMissingHearingDataFails()
        {
            var createOrderResult = CreateNewEnforcementOrderEntity(
                NewEnforcementOrderType.Proposed,
                _order.Cause,
                _order.CommentContactId,
                _order.CommentPeriodClosesDate,
                _order.County,
                _order.FacilityName,
                _order.ExecutedDate,
                _order.ExecutedOrderPostedDate,
                null,
                null,
                null,
                null,
                true,
                _order.LegalAuthorityId,
                _order.OrderNumber,
                _order.ProposedOrderPostedDate,
                _order.PublicationStatus,
                _order.Requirements,
                _order.SettlementAmount);

            createOrderResult.Success.Should().BeFalse();
            createOrderResult.NewItem.Should().BeNull();
            createOrderResult.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(4)
                .And.ContainKeys(new string[] {
                    "HearingDate",
                    "HearingLocation",
                    "HearingCommentPeriodClosesDate",
                    "HearingContact"
                });
        }

    }
}
