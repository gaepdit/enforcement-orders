using Enfo.API.Classes;
using Enfo.API.Controllers;
using Enfo.API.QueryStrings;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Repository.Querying;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Data;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Xunit;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.API.Tests.IntegrationTests
{
    public class OrdersPostIntegrationTests
    {
        private readonly EnforcementOrder[] _enforcementOrders;
        private readonly EpdContact[] _epdContacts;
        private readonly Address[] _addresses;
        private readonly LegalAuthority[] _legalAuthorities;

        public OrdersPostIntegrationTests()
        {
            _enforcementOrders = TestData.GetEnforcementOrders();
            _epdContacts = DomainData.GetEpdContacts();
            _addresses = DomainData.GetAddresses();
            _legalAuthorities = DomainData.GetLegalAuthorities();

            foreach (var contact in _epdContacts)
            {
                contact.Address = _addresses.SingleOrDefault(e => e.Id == contact.AddressId);
            }

            foreach (var order in _enforcementOrders)
            {
                order.LegalAuthority = _legalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
                order.CommentContact = order.CommentContactId.HasValue ? _epdContacts.SingleOrDefault(e => e.Id == order.CommentContactId) : null;
                order.HearingContact = order.HearingContactId.HasValue ? _epdContacts.SingleOrDefault(e => e.Id == order.HearingContactId) : null;
            }
        }

        [Fact]
        public async Task AddNewItemReturnsCorrectly()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var item = new EnforcementOrderCreate()
            {
                FacilityName = "TEST FACILITY",
                County = "Appling",
                LegalAuthorityId = 1,
                OrderNumber = "TEST-123",
                CreateAs = NewEnforcementOrderType.Executed
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<CreatedAtActionResult>();
            var actionResult = result as CreatedAtActionResult;
            actionResult.ActionName.Should().Be("Get");
            actionResult.StatusCode.Should().Be(201);
            actionResult.Value.Should().BeOfType<int>();
        }

        [Fact]
        public async Task AddNewItemCorrectlyAdds()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var item = new EnforcementOrderCreate()
            {
                FacilityName = "TEST FACILITY",
                County = "Appling",
                LegalAuthorityId = 1,
                OrderNumber = "TEST-123",
                CreateAs = NewEnforcementOrderType.Executed
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            int id = (int)(result as CreatedAtActionResult).Value;
            var addedItem = await repository
                .GetByIdAsync(id, inclusion: new EnforcementOrderIncludeAll())
                .ConfigureAwait(false);

            // Item gets added with next value in DB
            var expected = item.NewEnforcementOrder();
            expected.Id = _enforcementOrders.Max(e => e.Id) + 1;
            expected.LegalAuthority = _legalAuthorities.Single(e => e.Id == item.LegalAuthorityId);
            expected.HearingContact = _epdContacts.SingleOrDefault(e => e.Id == item.HearingContactId);
            expected.CommentContact = _epdContacts.SingleOrDefault(e => e.Id == item.CommentContactId);

            addedItem.Should().BeEquivalentTo(expected);

            // Verify repository has changed.
            var resultItems = await repository.ListAsync().ConfigureAwait(false);

            resultItems.Count.Should().Be(_enforcementOrders.Count() + 1);
        }

        [Fact]
        public async Task CreateWithDuplicateOrderNumberFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var item = new EnforcementOrderCreate()
            {
                FacilityName = "TEST FACILITY",
                County = "Appling",
                LegalAuthorityId = 1,
                OrderNumber = "EPD-ACQ-7936",
                CreateAs = NewEnforcementOrderType.Executed
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
            ((result as BadRequestObjectResult).Value as SerializableError)
                .Should().HaveCount(1)
                .And.ContainKey("OrderNumber");

            // Verify repository not changed after attempting to Post 
            // duplicate order number.
            var resultItems = (PaginatedList<EnforcementOrderSummaryView>)((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _enforcementOrders
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderSummaryView(e));

            resultItems.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateWithMultipleErrorsFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var item = new EnforcementOrderCreate()
            {
                FacilityName = "TEST FACILITY",
                County = "Appling",
                LegalAuthorityId = 1,
                OrderNumber = "EPD-ACQ-7936",
                CreateAs = NewEnforcementOrderType.Proposed,
                IsHearingScheduled = true,
                PublicationStatus = PublicationState.Published
            };

            var result = await controller.Post(item).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
            ((result as BadRequestObjectResult).Value as SerializableError)
                .Should().HaveCount(8)
                .And.ContainKeys(new string[] {
                    "HearingDate",
                    "OrderNumber",
                    "CommentContact",
                    "HearingContact",
                    "HearingLocation",
                    "CommentPeriodClosesDate",
                    "ProposedOrderPostedDate",
                    "HearingCommentPeriodClosesDate"
                });

            // Verify repository not changed after attempting to Post 
            // duplicate order number.
            var resultItems = (PaginatedList<EnforcementOrderSummaryView>)((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _enforcementOrders
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderSummaryView(e));

            resultItems.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddNullItemFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Post(null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            // Verify repository not changed after attempting to Post null item.
            var resultItems = (PaginatedList<EnforcementOrderSummaryView>)((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _enforcementOrders
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderSummaryView(e));

            resultItems.Items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateOrderSucceeds()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var id = 140;
            var target = new EnforcementOrderUpdate()
            {
                Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Liberty",
                ExecutedDate = new DateTime(1998, 06, 29),
                ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
                FacilityName = "A diam maecenas",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = null,
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                LegalAuthorityId = 7,
                OrderNumber = "EPD-ACQ-7936",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi."
                + Environment.NewLine
                + "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
                SettlementAmount = 2000
            };

            var result = await controller.Put(id, target);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult).StatusCode.Should().Be(204);

            var updated = await repository.GetByIdAsync(id)
                .ConfigureAwait(false);

            updated.Should().BeEquivalentTo(target);
        }

        [Fact]
        public async Task UpdateWithNullFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var id = 140;
            var original = await repository.GetByIdAsync(id).ConfigureAwait(false);

            var result = await controller.Put(id, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);

            var updated = await repository.GetByIdAsync(id).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(original);
        }

        [Fact]
        public async Task UpdateWithMissingIdFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var id = 9999;
            var target = new EnforcementOrderUpdate()
            {
                Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Liberty",
                ExecutedDate = new DateTime(1998, 06, 29),
                ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
                FacilityName = "A diam maecenas",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                LegalAuthorityId = 7,
                OrderNumber = "EPD-ACQ-7936",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi."
                + Environment.NewLine
                + "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
                SettlementAmount = 2000
            };

            var result = await controller.Put(id, target).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);
            (result as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task UpdateWithDuplicateOrderNumberFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var id = 140;
            var original = await repository.GetByIdAsync(id).ConfigureAwait(false);

            var target = new EnforcementOrderUpdate()
            {
                Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Liberty",
                ExecutedDate = new DateTime(1998, 06, 29),
                ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
                FacilityName = "A diam maecenas",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                LegalAuthorityId = 7,
                OrderNumber = "EPD-AQ-17310",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi."
                + Environment.NewLine
                + "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
                SettlementAmount = 2000
            };

            var result = await controller.Put(id, target).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).StatusCode.Should().Be(400);
            ((result as BadRequestObjectResult).Value as SerializableError)
            .Should().HaveCount(1);

            var updated = await repository.GetByIdAsync(id).ConfigureAwait(false);

            updated.Should().BeEquivalentTo(original);
        }

        [Fact]
        public async Task DeleteItemReturnsCorrectly()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Delete(140).ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            var actionResult = result as NoContentResult;
            actionResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task UnDeleteItemReturnsCorrectly()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Undelete(58310).ConfigureAwait(false);

            result.Should().BeOfType<NoContentResult>();
            var actionResult = result as NoContentResult;
            actionResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task DeleteItemCorrectlyDeletes()
        {
            int id = 140;

            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Delete(id).ConfigureAwait(false);

            // Verify item marked as deleted
            var updatedItem = await repository.GetByIdAsync(id).ConfigureAwait(false);
            updatedItem.Deleted.Should().BeTrue();

            // Verify removed from repository
            var originalExtantCount = _enforcementOrders.Where(e => !e.Deleted).Count();
            var newExtantCount = await repository.CountAsync(new FilterOrdersByDeletedStatus())
                .ConfigureAwait(false);

            newExtantCount.Should().Be(originalExtantCount - 1);
        }

        [Fact]
        public async Task UndeleteItemCorrectlyUndeletes()
        {
            int id = 58310;

            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Undelete(id).ConfigureAwait(false);

            // Verify item unmarked as deleted
            var updatedItem = await repository.GetByIdAsync(id).ConfigureAwait(false);
            updatedItem.Deleted.Should().BeFalse();

            // Verify added back to repository
            var originalExtantCount = _enforcementOrders.Where(e => !e.Deleted).Count();
            var newExtantCount = await repository.CountAsync(new FilterOrdersByDeletedStatus())
                .ConfigureAwait(false);

            newExtantCount.Should().Be(originalExtantCount + 1);
        }

        [Fact]
        public async Task DeleteOfAlreadyDeletedOrderFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Delete(58310).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            var actionResult = result as BadRequestObjectResult;
            actionResult.StatusCode.Should().Be(400);

            // Verify repository unchanged
            var originalExtantCount = _enforcementOrders.Where(e => !e.Deleted).Count();
            var newExtantCount = await repository.CountAsync(new FilterOrdersByDeletedStatus())
                .ConfigureAwait(false);

            newExtantCount.Should().Be(originalExtantCount);
        }

        [Fact]
        public async Task UndeleteOfNonDeletedOrderFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Undelete(140).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            var actionResult = result as BadRequestObjectResult;
            actionResult.StatusCode.Should().Be(400);

            // Verify repository unchanged
            var originalExtantCount = _enforcementOrders.Where(e => !e.Deleted).Count();
            var newExtantCount = await repository.CountAsync(new FilterOrdersByDeletedStatus())
                .ConfigureAwait(false);

            newExtantCount.Should().Be(originalExtantCount);
        }

        [Fact]
        public async Task DeleteWithNonexistantIdFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            int id = -1;

            var result = await controller.Delete(id).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);

            // Verify repository unchanged
            var originalExtantCount = _enforcementOrders.Where(e => !e.Deleted).Count();
            var newExtantCount = await repository.CountAsync(new FilterOrdersByDeletedStatus())
                .ConfigureAwait(false);

            newExtantCount.Should().Be(originalExtantCount);
        }

        [Fact]
        public async Task UndeleteWithNonexistantIdFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            int id = -1;

            var result = await controller.Undelete(id).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
            (result as NotFoundObjectResult).StatusCode.Should().Be(404);

            // Verify repository unchanged
            var originalExtantCount = _enforcementOrders.Where(e => !e.Deleted).Count();
            var newExtantCount = await repository.CountAsync(new FilterOrdersByDeletedStatus())
                .ConfigureAwait(false);

            newExtantCount.Should().Be(originalExtantCount);
        }
    }
}
