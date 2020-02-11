using Enfo.API.Controllers;
using Enfo.API.QueryStrings;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Enfo.Domain.Entities.EnforcementOrder;
using static Enfo.Domain.Utils.DateUtils;

namespace Enfo.API.Tests.IntegrationTests
{
    public class OrdersExtraIntegrationTests
    {
        private readonly EnforcementOrder[] _orders;

        public OrdersExtraIntegrationTests()
        {
            _orders = DevSeedData.GetEnforcementOrders();

            var epdContacts = ProdSeedData.GetEpdContacts();
            var addresses = ProdSeedData.GetAddresses();
            var legalAuthorities = ProdSeedData.GetLegalAuthorities();

            foreach (var contact in epdContacts)
            {
                contact.Address = addresses.SingleOrDefault(e => e.Id == contact.AddressId);
            }

            foreach (var order in _orders)
            {
                order.LegalAuthority = legalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
                order.CommentContact = epdContacts.SingleOrDefault(e => e.Id == order.CommentContactId);
                order.HearingContact = epdContacts.SingleOrDefault(e => e.Id == order.HearingContactId);
            }
        }

        [Fact]
        public async Task GetDefaultCountReturnsCorrectCount()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Count()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _orders.Where(e => !e.Deleted).Count();

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("diam")]
        [InlineData("orci")]
        public async Task GetFilteredCountReturnsCorrectCount(string facilityFilter)
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var value = ((await controller.Count(
                new EnforcementOrderFilter() { FacilityFilter = facilityFilter })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => e.FacilityName.ToLower().Contains(facilityFilter.ToLower()))
                .Where(e => !e.Deleted)
                .Count();

            value.Should().Be(expected);
        }

        [Fact]
        public async Task GetDeletedCountReturnsCorrectCount()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Count(
                new EnforcementOrderFilter() { Deleted = true })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => e.Deleted)
                .Count();

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Cherokee")]
        [InlineData("stephens")]
        public async Task GetFilteredDeletedCountReturnsCorrectCount(string county)
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var value = ((await controller.Count(
                new EnforcementOrderFilter() { County = county, Deleted = true })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => e.County.ToLower().Contains(county.ToLower()))
                .Where(e => e.Deleted)
                .Count();

            value.Should().Be(expected);
        }

        [Fact]
        public async Task GetCurrentProposedReturnsCorrectItems()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.CurrentProposed()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => e.GetIsPublicProposedOrder() && e.CommentPeriodClosesDate >= DateTime.Today)
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetCurrentProposedPaginatedReturnsCorrectItems(
            int pageSize,
            int pageNumber)
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.CurrentProposed(
                new PaginationFilter() { PageSize = pageSize, Page = pageNumber })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => e.GetIsPublicProposedOrder() && e.CommentPeriodClosesDate >= DateTime.Today)
                .Where(e => !e.Deleted)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetRecentlyExecutedReturnsCorrectItems()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.RecentlyExecuted()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            // fromDate is most recent Monday
            var fromDate = GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday);

            var expected = _orders
                .Where(e => e.GetIsPublicExecutedOrder()
                    && e.ExecutedOrderPostedDate >= fromDate
                    && e.ExecutedOrderPostedDate <= DateTime.Today)
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetRecentlyExecutedPaginatedReturnsCorrectItems(
            int pageSize,
            int pageNumber)
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.RecentlyExecuted(
                new PaginationFilter() { PageSize = pageSize, Page = pageNumber })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            // fromDate is most recent Monday
            var fromDate = GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday);

            var expected = _orders
                .Where(e => e.GetIsPublicExecutedOrder()
                    && e.ExecutedOrderPostedDate >= fromDate
                    && e.ExecutedOrderPostedDate <= DateTime.Today)
                .Where(e => !e.Deleted)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetDraftsReturnsCorrectItems()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Drafts()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => e.PublicationStatus == PublicationState.Draft)
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetDraftsPaginatedReturnsCorrectItems(
            int pageSize,
            int pageNumber)
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Drafts(
                new PaginationFilter() { PageSize = pageSize, Page = pageNumber })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => e.PublicationStatus == PublicationState.Draft)
                .Where(e => !e.Deleted)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPendingReturnsCorrectItems()
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Pending()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => e.GetIsPublic()
                    && e.GetLastPostedDate() > GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday))
                .Where(e => !e.Deleted)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetPendingPaginatedReturnsCorrectItems(
            int pageSize,
            int pageNumber)
        {
            var repository = this.GetEnforcementOrderRepository();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Pending(
                new PaginationFilter() { PageSize = pageSize, Page = pageNumber })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _orders
                .Where(e => (e.GetIsPublic())
                    && e.GetLastPostedDate() > GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday))
                .Where(e => !e.Deleted)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }
    }
}
