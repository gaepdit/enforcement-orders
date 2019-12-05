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

namespace Enfo.API.Tests.ControllerTests
{
    public class EnforcementOrdersControllerOtherTests
    {
        private readonly EnforcementOrder[] _allOrders;

        public EnforcementOrdersControllerOtherTests()
        {
            _allOrders = DevSeedData.GetEnforcementOrders();

            var epdContacts = ProdSeedData.GetEpdContacts();
            var addresses = ProdSeedData.GetAddresses();
            var legalAuthorities = ProdSeedData.GetLegalAuthorities();

            foreach (var contact in epdContacts)
            {
                contact.Address = addresses.SingleOrDefault(e => e.Id == contact.AddressId);
            }

            foreach (var order in _allOrders)
            {
                order.LegalAuthority = legalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
                order.CommentContact = epdContacts.SingleOrDefault(e => e.Id == order.CommentContactId);
                order.HearingContact = epdContacts.SingleOrDefault(e => e.Id == order.HearingContactId);
            }
        }

        [Fact]
        public async Task GetDefaultCountReturnsCorrectCount()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Count()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Count();

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("diam")]
        [InlineData("orci")]
        public async Task GetFilteredCountReturnsCorrectCount(string facilityFilter)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var value = ((await controller.Count(
                new EnforcementOrderFilter() { FacilityFilter = facilityFilter })
                .ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => e.FacilityName.ToLower().Contains(facilityFilter.ToLower()))
                .Count();

            value.Should().Be(expected);
        }

        [Fact]
        public async Task GetCurrentProposedReturnsCorrectItems()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.CurrentProposed()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => e.IsPublicProposedOrder
                && e.CommentPeriodClosesDate >= DateTime.Today)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetCurrentProposedPaginatedReturnsCorrectItems(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.CurrentProposed(
                new PaginationFilter() { PageSize = pageSize, Page = pageNum })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => e.IsPublicProposedOrder
                && e.CommentPeriodClosesDate >= DateTime.Today)
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetRecentlyExecutedReturnsCorrectItems()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.RecentlyExecuted()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            // fromDate is most recent Monday
            var fromDate = GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday);

            var expected = _allOrders.Where(
                e => e.IsPublicExecutedOrder
                && e.ExecutedOrderPostedDate >= fromDate
                && e.ExecutedOrderPostedDate <= DateTime.Today)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetRecentlyExecutedPaginatedReturnsCorrectItems(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.RecentlyExecuted(
                new PaginationFilter() { PageSize = pageSize, Page = pageNum })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            // fromDate is most recent Monday
            var fromDate = GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday);

            var expected = _allOrders.Where(
                e => e.IsPublicExecutedOrder
                && e.ExecutedOrderPostedDate >= fromDate
                && e.ExecutedOrderPostedDate <= DateTime.Today)
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetDraftsReturnsCorrectItems()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Drafts()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => e.PublicationStatus == PublicationState.Draft)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetDraftsPaginatedReturnsCorrectItems(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Drafts(
                new PaginationFilter() { PageSize = pageSize, Page = pageNum })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => e.PublicationStatus == PublicationState.Draft)
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPendingReturnsCorrectItems()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Pending()
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => (e.IsPublic)
                && e.LastPostedDate > GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday))
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetPendingPaginatedReturnsCorrectItems(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Pending(
                new PaginationFilter() { PageSize = pageSize, Page = pageNum })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => (e.IsPublic)
                && e.LastPostedDate > GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday))
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }
    }
}
