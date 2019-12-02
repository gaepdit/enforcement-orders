
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
using static Enfo.Domain.Entities.Enums;

namespace Enfo.API.Tests.ControllerTests
{
    public class EnforcementOrdersControllerGetTests
    {
        private readonly EnforcementOrder[] _allOrders;

        public EnforcementOrdersControllerGetTests()
        {
            _allOrders = DevSeedData.GetEnforcementOrders();
        }

        [Fact]
        public async Task GetReturnsOkAsync()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result;

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetReturnsCorrectTypeAsync()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get().ConfigureAwait(false))
                .Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<EnforcementOrderResource>>(result.Value);
        }

        [Fact]
        public async Task GetReturnsAllItemsAsync()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = _allOrders
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(EnforcementOrderSorting.FacilityAsc, 2, 2)]
        [InlineData(EnforcementOrderSorting.FacilityDesc, 2, 2)]
        [InlineData(EnforcementOrderSorting.DateAsc, 2, 2)]
        [InlineData(EnforcementOrderSorting.DateDesc, 2, 2)]
        public async Task GetPaginatedAndSortedReturnsSomeItemsAsync(
            EnforcementOrderSorting sortOrder,
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            int firstItemIndex = (pageNum - 1) * pageSize;

            var result = (await controller.Get(
                sortOrder: sortOrder,
                pageSize: pageSize,
                page: pageNum)
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var orderedOrders = sortOrder switch
            {
                EnforcementOrderSorting.DateAsc => _allOrders.OrderBy(e => e.LastPostedDate),
                EnforcementOrderSorting.DateDesc => _allOrders.OrderByDescending(e => e.LastPostedDate),
                EnforcementOrderSorting.FacilityAsc => _allOrders.OrderBy(e => e.FacilityName),
                EnforcementOrderSorting.FacilityDesc => _allOrders.OrderByDescending(e => e.FacilityName),
                _ => throw new ArgumentException()
            };

            var expected = orderedOrders
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageSizeReturnsDefaultPaginationAsync()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: -1)
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageNumberReturnsDefaultPaginationAsync()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(page: 0)
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("diam")]
        [InlineData("orci")]
        public async Task FacilityFilterReturnsCorrectItemsAsync(string facilityFilter)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                facilityFilter: facilityFilter)
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = Array.FindAll(_allOrders,
                e => e.FacilityName.ToLower().Contains(facilityFilter.ToLower()))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Cherokee")]
        [InlineData("Stephens")]
        public async Task CountyFilterReturnsCorrectItemsAsync(string county)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                county: county)
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = Array.FindAll(_allOrders, e => e.County.Equals(county))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(6)]
        public async Task LegalFilterReturnsCorrectItemsAsync(int legalAuth)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                legalAuth: legalAuth
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = Array.FindAll(_allOrders,
                e => e.LegalAuthorityId.Equals(legalAuth))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        public static IEnumerable<object[]> DateFilterData
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { new DateTime(2000, 01, 01), ActivityStatus.All },
                    new object[] { new DateTime(1998, 01, 01), ActivityStatus.Executed },
                    new object[] { new DateTime(2018, 01, 01), ActivityStatus.Proposed }
                };
            }
        }

        [Theory, MemberData(nameof(DateFilterData))]
        public async Task DateFromFilterReturnsCorrectItemsAsync(
            DateTime fromDate,
            ActivityStatus status)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                fromDate: fromDate,
                status: status
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = Array.FindAll(_allOrders, e =>
                (status == ActivityStatus.All
                || (status == ActivityStatus.Executed && e.IsExecutedOrder)
                || (status == ActivityStatus.Proposed && e.IsProposedOrder && !e.IsExecutedOrder))
                && (status == ActivityStatus.All && (e.ProposedOrderPostedDate >= fromDate || e.ExecutedDate >= fromDate)
                || (status == ActivityStatus.Executed && e.ExecutedDate >= fromDate)
                || (status == ActivityStatus.Proposed && e.ProposedOrderPostedDate >= fromDate)))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(PublicationStatus.All)]
        [InlineData(PublicationStatus.Draft)]
        [InlineData(PublicationStatus.Published)]
        public async Task PubStatusFilterReturnsCorrectItemsAsync(PublicationStatus publicationStatus)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                publicationStatus: publicationStatus
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = Array.FindAll(_allOrders,
                e => (publicationStatus == PublicationStatus.All
                || (publicationStatus == PublicationStatus.Draft && e.PublicationStatus == EnforcementOrder.PublicationState.Draft)
                || (publicationStatus == PublicationStatus.Published && e.PublicationStatus == EnforcementOrder.PublicationState.Published)))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory, MemberData(nameof(DateFilterData))]
        public async Task DateTillFilterReturnsCorrectItemsAsync(
            DateTime tillDate,
            ActivityStatus status = ActivityStatus.All)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                tillDate: tillDate,
                status: status
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = Array.FindAll(_allOrders,
                e => (status == ActivityStatus.All
                || (status == ActivityStatus.Executed && e.IsExecutedOrder)
                || (status == ActivityStatus.Proposed && e.IsProposedOrder && !e.IsExecutedOrder))
                && (status == ActivityStatus.All && (e.ProposedOrderPostedDate <= tillDate || e.ExecutedDate <= tillDate)
                || (status == ActivityStatus.Executed && e.ExecutedDate <= tillDate)
                || (status == ActivityStatus.Proposed && e.ProposedOrderPostedDate <= tillDate)))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("EPD-WP")]
        [InlineData("8")]
        public async Task OrderNumberFilterReturnsCorrectItemsAsync(string orderNumber)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                orderNumber: orderNumber
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = Array.FindAll(_allOrders,
                e => e.OrderNumber.Contains(orderNumber))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("nunc")]
        [InlineData("nulla")]
        [InlineData("fāċilisi")]
        [InlineData("👾🔙")]
        public async Task TextFilterReturnsCorrectItemsAsync(string textContains)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                textContains: textContains,
                publicationStatus: PublicationStatus.All
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderResource>;

            var expected = Array.FindAll(_allOrders,
                e => e.Cause.ToLower().Contains(textContains.ToLower())
                || e.Requirements.ToLower().Contains(textContains.ToLower()))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(140)]
        [InlineData(27)]
        [InlineData(71715)]
        public async Task GetByIdReturnsCorrectItemAsync(int id)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var value = (await controller.Get(id).ConfigureAwait(false))
                .Value;

            var expected = new EnforcementOrderResource(Array.Find(_allOrders,
                e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFoundAsync(int id)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(id).ConfigureAwait(false))
                .Value;

            result.Should().BeNull();
        }
    }
}
