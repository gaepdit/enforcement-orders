using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using Enfo.Infrastructure.Tests.Helpers;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.Infrastructure.Tests
{
    public class EnforcementOrderRepositoryTests
    {
        private readonly EnforcementOrder[] _orders;

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
            OrderNumber = "EPD-NEW-9999",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            PublicationStatus = PublicationState.Published,
            Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
            SettlementAmount = 2000
        };

        public EnforcementOrderRepositoryTests()
        {
            _orders = DevSeedData.GetEnforcementOrders();
        }

        [Fact]
        public async Task CreateNewOrderSucceeds()
        {
            using var repository = this.GetEnforcementOrdersRepository();

            var result = await repository.CreateEnforcementOrderAsync(
                NewEnforcementOrderType.Executed, _order.Cause, _order.CommentContactId, _order.CommentPeriodClosesDate,
                _order.County, _order.FacilityName, _order.ExecutedDate, _order.ExecutedOrderPostedDate,
                _order.HearingCommentPeriodClosesDate, _order.HearingContactId, _order.HearingDate,
                _order.HearingLocation, _order.IsHearingScheduled, _order.LegalAuthorityId, _order.OrderNumber,
                _order.ProposedOrderPostedDate, _order.PublicationStatus, _order.Requirements, _order.SettlementAmount);

            result.Success.Should().BeTrue();

            int postCount = await repository.CountAsync().ConfigureAwait(false);

            postCount.Should().Be(_orders.Length + 1);

            var addedItem = await repository.GetByIdAsync(result.NewItem.Id).ConfigureAwait(false);

            addedItem.Should().BeEquivalentTo(result.NewItem);
        }

        [Fact]
        public async Task CreateOrderWithDuplicateOrderNumberFails()
        {
            using var repository = this.GetEnforcementOrdersRepository();

            var result = await repository.CreateEnforcementOrderAsync(
                NewEnforcementOrderType.Executed, _order.Cause, _order.CommentContactId, _order.CommentPeriodClosesDate,
                _order.County, _order.FacilityName, _order.ExecutedDate, _order.ExecutedOrderPostedDate,
                _order.HearingCommentPeriodClosesDate, _order.HearingContactId, _order.HearingDate,
                _order.HearingLocation, _order.IsHearingScheduled, _order.LegalAuthorityId,
                "EPD-ACQ-7937",
                _order.ProposedOrderPostedDate, _order.PublicationStatus, _order.Requirements, _order.SettlementAmount);

            result.Success.Should().BeFalse();
            result.NewItem.Should().BeNull();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainKey("OrderNumber");
        }

        [Fact]
        public async Task UpdateOrderSucceeds()
        {
            using var repository = this.GetEnforcementOrdersRepository();

            var id = 140;
            var thisItem = await repository.GetByIdAsync(id).ConfigureAwait(false);

            var result = await repository.UpdateEnforcementOrderAsync(
                id, thisItem.Cause, thisItem.CommentContactId, thisItem.CommentPeriodClosesDate, thisItem.County,
                thisItem.FacilityName, thisItem.ExecutedDate, thisItem.ExecutedOrderPostedDate,
                thisItem.HearingCommentPeriodClosesDate, thisItem.HearingContactId, thisItem.HearingDate,
                thisItem.HearingLocation, thisItem.IsExecutedOrder, thisItem.IsHearingScheduled, thisItem.LegalAuthorityId,
                thisItem.OrderNumber, thisItem.ProposedOrderPostedDate, thisItem.PublicationStatus, thisItem.Requirements,
                thisItem.SettlementAmount).ConfigureAwait(false);

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateDeletedOrderFails()
        {
            using var repository = this.GetEnforcementOrdersRepository();

            var id = 58310;
            var thisItem = await repository.GetByIdAsync(id).ConfigureAwait(false);

            var result = await repository.UpdateEnforcementOrderAsync(
                id, _order.Cause, _order.CommentContactId, _order.CommentPeriodClosesDate, _order.County,
                _order.FacilityName, _order.ExecutedDate, _order.ExecutedOrderPostedDate,
                _order.HearingCommentPeriodClosesDate, _order.HearingContactId, _order.HearingDate,
                _order.HearingLocation, _order.IsExecutedOrder, _order.IsHearingScheduled, _order.LegalAuthorityId,
                _order.OrderNumber, _order.ProposedOrderPostedDate, _order.PublicationStatus, _order.Requirements,
                _order.SettlementAmount);

            result.Success.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainKey("Id");
        }

        [Fact]
        public async Task UpdateOrderWithDuplicateOrderNumberFails()
        {
            using var repository = this.GetEnforcementOrdersRepository();

            var id = 140;
            var thisItem = await repository.GetByIdAsync(id).ConfigureAwait(false);

            var result = await repository.UpdateEnforcementOrderAsync(
                id, _order.Cause, _order.CommentContactId, _order.CommentPeriodClosesDate, _order.County,
                _order.FacilityName, _order.ExecutedDate, _order.ExecutedOrderPostedDate,
                _order.HearingCommentPeriodClosesDate, _order.HearingContactId, _order.HearingDate,
                _order.HearingLocation, _order.IsExecutedOrder, _order.IsHearingScheduled, _order.LegalAuthorityId,
                "EPD-ACQ-7937",
                _order.ProposedOrderPostedDate, _order.PublicationStatus, _order.Requirements, _order.SettlementAmount);

            result.Success.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainKey("OrderNumber");

            var updatedItem = await repository.GetByIdAsync(id).ConfigureAwait(false);

            updatedItem.Should().BeEquivalentTo(thisItem);
        }
    }
}
