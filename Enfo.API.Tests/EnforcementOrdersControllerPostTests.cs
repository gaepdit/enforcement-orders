using Enfo.API.Controllers;
using Enfo.API.QueryStrings;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.API.Tests.ControllerTests
{
    public class EnforcementOrdersControllerPostTests
    {
        private readonly EnforcementOrder[] _enforcementOrders;
        private readonly EpdContact[] _epdContacts;
        private readonly Address[] _addresses;
        private readonly LegalAuthority[] _legalAuthorities;

        public EnforcementOrdersControllerPostTests()
        {
            _enforcementOrders = DevSeedData.GetEnforcementOrders();
            _epdContacts = ProdSeedData.GetEpdContacts();
            _addresses = ProdSeedData.GetAddresses();
            _legalAuthorities = ProdSeedData.GetLegalAuthorities();

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

            var item = new EnforcementOrderCreateResource()
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

            var item = new EnforcementOrderCreateResource()
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

            var item = new EnforcementOrderCreateResource()
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
            var resultItems = ((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _enforcementOrders
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderListResource(e));

            resultItems.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateWithMultipleErrorsFails()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var item = new EnforcementOrderCreateResource()
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
            var resultItems = ((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _enforcementOrders
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderListResource(e));

            resultItems.Should().BeEquivalentTo(expected);
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
            var resultItems = ((await controller.Get(
                paging: new PaginationFilter() { PageSize = 0 })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _enforcementOrders
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderListResource(e));

            resultItems.Should().BeEquivalentTo(expected);
        }
    }
}
