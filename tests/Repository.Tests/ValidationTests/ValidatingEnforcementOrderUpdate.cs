using System;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources.EnforcementOrder;
using FluentAssertions;
using Xunit;
using static Enfo.Repository.Validation.EnforcementOrderValidation;

namespace Repository.Tests.ValidationTests
{
    public class ValidatingEnforcementOrderUpdate
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
            Id = 1,
            IsExecutedOrder = true,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 7,
            OrderNumber = "EPD-ACQ-7936",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements =
                "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
            SettlementAmount = 2000,
        };

        private readonly EnforcementOrderUpdate _orderUpdate = new EnforcementOrderUpdate()
        {
            Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
            CommentContactId = 2004,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "A diam maecenas",
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2004,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "venenatis urna cursus eget nunc scelerisque viverra mauris in aliquam sem",
            IsExecutedOrder = true,
            IsHearingScheduled = true,
            LegalAuthorityId = 7,
            OrderNumber = "EPD-ACQ-7936",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            PublicationStatus = PublicationState.Published,
            Requirements =
                "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
            SettlementAmount = 2000,
        };

        [Fact]
        public void SucceedsGivenValidUpdates()
        {
            ValidateEnforcementOrderUpdate(_order, _orderUpdate).IsValid.Should().BeTrue();
        }

        [Fact]
        public void FailsGivenDeletedOrder()
        {
            _order.Deleted = true;

            var result = ValidateEnforcementOrderUpdate(_order, _orderUpdate);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainKey("Id");
        }

        [Fact]
        public void SucceedsWhenRemovingExecutedOrderGivenProposedOrder()
        {
            _orderUpdate.ExecutedDate = null;
            _orderUpdate.ExecutedOrderPostedDate = null;
            _orderUpdate.IsExecutedOrder = false;

            var result = ValidateEnforcementOrderUpdate(_order, _orderUpdate);

            result.IsValid.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void FailsWhenRemovingExecutedOrderIfNotProposedOrder()
        {
            _order.CommentContactId = null;
            _order.CommentPeriodClosesDate = null;
            _order.IsProposedOrder = false;
            _order.ProposedOrderPostedDate = null;

            _orderUpdate.ExecutedDate = null;
            _orderUpdate.ExecutedOrderPostedDate = null;
            _orderUpdate.IsExecutedOrder = false;

            var result = ValidateEnforcementOrderUpdate(_order, _orderUpdate);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainKey("IsExecutedOrder");
        }

        [Fact]
        public void FailsWhenRemovingExecutedOrderDetails()
        {
            _orderUpdate.ExecutedDate = null;
            _orderUpdate.ExecutedOrderPostedDate = null;

            var result = ValidateEnforcementOrderUpdate(_order, _orderUpdate);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.ContainKeys("ExecutedDate", "ExecutedOrderPostedDate");
        }

        [Fact]
        public void SucceedsWhenRemovingHearing()
        {
            _orderUpdate.IsHearingScheduled = false;

            var result = ValidateEnforcementOrderUpdate(_order, _orderUpdate);

            result.IsValid.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void FailsWhenRemovingHearingDetailsIfHearingStillTrue()
        {
            _orderUpdate.HearingCommentPeriodClosesDate = null;
            _orderUpdate.HearingContactId = null;
            _orderUpdate.HearingDate = null;
            _orderUpdate.HearingLocation = null;

            var result = ValidateEnforcementOrderUpdate(_order, _orderUpdate);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(4)
                .And.ContainKeys(
                    "HearingDate",
                    "HearingLocation",
                    "HearingCommentPeriodClosesDate",
                    "HearingContact");
        }
    }
}