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
        public async Task GetReturnsCorrectly()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Get().ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<EnforcementOrderListResource>>(actionResult.Value);
            actionResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetReturnsAllItems()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Get(pageSize: 0)
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

            var expected = _allOrders
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(EnforcementOrderSorting.FacilityAsc, 3, 2)]
        [InlineData(EnforcementOrderSorting.FacilityDesc, 3, 2)]
        [InlineData(EnforcementOrderSorting.DateAsc, 3, 2)]
        [InlineData(EnforcementOrderSorting.DateDesc, 3, 2)]
        public async Task GetPaginatedAndSortedReturnsSomeItems(
            EnforcementOrderSorting sortOrder,
            int pageSize,
            int pageNum)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var items = ((await controller.Get(
                sortOrder: sortOrder,
                pageSize: pageSize,
                page: pageNum)
                .ConfigureAwait(false)).Result as OkObjectResult).Value;

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
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageSizeReturnsDefaultPagination()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: -1)
                .ConfigureAwait(false)).Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task InvalidPageNumberReturnsDefaultPagination()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(page: 0)
                .ConfigureAwait(false)).Result as OkObjectResult;

            var expected = (await controller.Get()
                .ConfigureAwait(false)).Result as OkObjectResult;

            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("diam")]
        [InlineData("orci")]
        public async Task FacilityFilterReturnsCorrectItems(string facilityFilter)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                facilityFilter: facilityFilter)
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderListResource>;

            var expected = _allOrders.Where(
                e => e.FacilityName.ToLower().Contains(facilityFilter.ToLower()))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Cherokee")]
        [InlineData("Stephens")]
        public async Task CountyFilterReturnsCorrectItems(string county)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                county: county)
                .ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderListResource>;

            var expected = _allOrders.Where(
                e => e.County.Equals(county))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(6)]
        public async Task LegalFilterReturnsCorrectItems(int legalAuth)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                legalAuth: legalAuth
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderListResource>;

            var expected = _allOrders.Where(
                e => e.LegalAuthorityId.Equals(legalAuth))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

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
        public async Task DateFromFilterReturnsCorrectItems(
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

            var items = result.Value as IEnumerable<EnforcementOrderListResource>;

            var expected = _allOrders.Where(
                e => (status == ActivityStatus.All
                || (status == ActivityStatus.Executed && e.IsExecutedOrder)
                || (status == ActivityStatus.Proposed && e.IsProposedOrder && !e.IsExecutedOrder))
                && (status == ActivityStatus.All && (e.ProposedOrderPostedDate >= fromDate || e.ExecutedDate >= fromDate)
                || (status == ActivityStatus.Executed && e.ExecutedDate >= fromDate)
                || (status == ActivityStatus.Proposed && e.ProposedOrderPostedDate >= fromDate)))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(PublicationStatus.All)]
        [InlineData(PublicationStatus.Draft)]
        [InlineData(PublicationStatus.Published)]
        public async Task PubStatusFilterReturnsCorrectItems(PublicationStatus publicationStatus)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                publicationStatus: publicationStatus
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderListResource>;

            var expected = _allOrders.Where(
                e => (publicationStatus == PublicationStatus.All
                || (publicationStatus == PublicationStatus.Draft && e.PublicationStatus == EnforcementOrder.PublicationState.Draft)
                || (publicationStatus == PublicationStatus.Published && e.PublicationStatus == EnforcementOrder.PublicationState.Published)))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory, MemberData(nameof(DateFilterData))]
        public async Task DateTillFilterReturnsCorrectItems(
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

            var items = result.Value as IEnumerable<EnforcementOrderListResource>;

            var expected = _allOrders.Where(
                e => (status == ActivityStatus.All
                || (status == ActivityStatus.Executed && e.IsExecutedOrder)
                || (status == ActivityStatus.Proposed && e.IsProposedOrder && !e.IsExecutedOrder))
                && (status == ActivityStatus.All && (e.ProposedOrderPostedDate <= tillDate || e.ExecutedDate <= tillDate)
                || (status == ActivityStatus.Executed && e.ExecutedDate <= tillDate)
                || (status == ActivityStatus.Proposed && e.ProposedOrderPostedDate <= tillDate)))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("EPD-WP")]
        [InlineData("8")]
        public async Task OrderNumberFilterReturnsCorrectItems(string orderNumber)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                orderNumber: orderNumber
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderListResource>;

            var expected = _allOrders.Where(
                e => e.OrderNumber.Contains(orderNumber))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("nunc")]
        [InlineData("nulla")]
        [InlineData("fāċilisi")]
        [InlineData("👾🔙")]
        public async Task TextFilterReturnsCorrectItems(string textContains)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = (await controller.Get(pageSize: 0,
                textContains: textContains,
                publicationStatus: PublicationStatus.All
                ).ConfigureAwait(false))
                .Result as OkObjectResult;

            var items = result.Value as IEnumerable<EnforcementOrderListResource>;

            var expected = _allOrders.Where(
                e => e.Cause.ToLower().Contains(textContains.ToLower())
                || e.Requirements.ToLower().Contains(textContains.ToLower()))
                .OrderBy(e => e.FacilityName)
                .Select(e => new EnforcementOrderListResource(e));

            items.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectly()
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Get(140).ConfigureAwait(false);

            result.Result.Should().BeOfType<OkObjectResult>();
            var actionResult = result.Result as OkObjectResult;
            actionResult.Value.Should().BeOfType<EnforcementOrderItemResource>();
            actionResult.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(140)]
        [InlineData(27)]
        [InlineData(71715)]
        public async Task GetByIdReturnsCorrectItem(int id)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var value = ((await controller.Get(id).ConfigureAwait(false))
                .Result as OkObjectResult).Value;

            var expected = new EnforcementOrderItemResource(
                _allOrders.Single(e => e.Id == id));

            value.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByMissingIdReturnsNotFound(int id)
        {
            var repository = this.GetRepository<EnforcementOrder>();
            var controller = new EnforcementOrdersController(repository);

            var result = await controller.Get(id).ConfigureAwait(false);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Value.Should().BeNull();
            (result.Result as NotFoundResult).StatusCode.Should().Be(404);
        }
    }
}
