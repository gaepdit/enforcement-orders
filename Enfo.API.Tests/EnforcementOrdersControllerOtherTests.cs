using Enfo.API.Controllers;
using Enfo.API.Resources;
using Enfo.API.Tests.Helpers;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
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
        public async Task GetCountReturnsCorrectCountAsync(string facilityFilter)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Count(
                facilityFilter: facilityFilter
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var expected = Array.FindAll(_allOrders,
                e => e.FacilityName.Contains(facilityFilter)).Count();

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 2)]
        public async Task GetCurrentProposedReturnsCorrectItemsAsync(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            int firstItemIndex = (pageNum - 1) * pageSize;

            var result = (await controller.CurrentProposed(
                pageSize: pageSize,
                page: pageNum
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            items.Should().HaveCount(pageSize);

            var expected = _allOrders.Where(e => e.IsPublicProposedOrder
                && e.CommentPeriodClosesDate >= DateTime.Today)
                .Select(e => new EnforcementOrderResource(e))
                .ToArray()[firstItemIndex];

            items.Should().HaveCount(pageSize);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetRecentlyExecutedReturnsCorrectItemsAsync()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.RecentlyExecuted().ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            // fromDate is most recent Monday
            var fromDate = GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday);

            var expected = Array.FindAll(_allOrders,
                e => e.IsPublicExecutedOrder
                && e.ExecutedOrderPostedDate >= fromDate
                && e.ExecutedOrderPostedDate <= DateTime.Today)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().HaveCount(expected.Count());
            items.ToList()[0].Should().BeEquivalentTo(expected.ToArray()[0]);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 2)]
        public async Task GetDraftsReturnsCorrectItemsAsync(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            int firstItemIndex = (pageNum - 1) * pageSize;

            var result = (await controller.Drafts(
                pageSize: pageSize,
                page: pageNum
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = _allOrders.Where(e => e.PublicationStatus == EnforcementOrder.PublicationState.Draft)
                .Select(e => new EnforcementOrderResource(e))
                .ToArray()[firstItemIndex];

            items.Should().HaveCount(pageSize);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 2)]
        public async Task GetPendingReturnsCorrectItemsAsync(
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            int firstItemIndex = (pageNum - 1) * pageSize;

            var result = (await controller.Pending(
                pageSize: pageSize,
                page: pageNum
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = _allOrders.Where(
                e => (e.IsPublicExecutedOrder || e.IsPublicProposedOrder)
                && e.LastPostedDate > GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday))
                .Select(e => new EnforcementOrderResource(e))
                .ToArray()[firstItemIndex];

            items.Should().HaveCount(pageSize);
            items.ToList()[0].Should().BeEquivalentTo(expected);
        }
    }
}
