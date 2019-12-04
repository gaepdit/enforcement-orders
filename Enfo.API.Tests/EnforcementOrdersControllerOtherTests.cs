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
        }

        [Theory]
        [InlineData("diam")]
        [InlineData("orci")]
        public async Task GetCountReturnsCorrectCount(string facilityFilter)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var value = ((await controller.Count(
                facilityFilter: facilityFilter
                ).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => e.FacilityName.Contains(facilityFilter))
                .Count();

            value.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 2)]
        public async Task GetCurrentProposedReturnsCorrectItems(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.CurrentProposed(
                filter: new PaginationFilter() { PageSize = pageSize, Page = pageNum })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => e.IsPublicProposedOrder
                && e.CommentPeriodClosesDate >= DateTime.Today)
                .Select(e => new EnforcementOrderItemResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 2)]
        public async Task GetRecentlyExecutedReturnsCorrectItems(
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
                .Select(e => new EnforcementOrderItemResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 2)]
        public async Task GetDraftsReturnsCorrectItems(
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
                .Select(e => new EnforcementOrderItemResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 2)]
        public async Task GetPendingReturnsCorrectItems(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Pending(
                new PaginationFilter() { PageSize = pageSize, Page = pageNum })
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders.Where(
                e => (e.IsPublicExecutedOrder || e.IsPublicProposedOrder)
                && e.LastPostedDate > GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday))
                .Select(e => new EnforcementOrderItemResource(e));

            items.Should().BeEquivalentTo(expected);
        }
    }
}
