using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.Domain.Specs;
using Enfo.WebApp.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Api
{
    public class ApiTests
    {
        [Fact]
        public async Task ListOrders_ReturnsPublicItems()
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();

            var controller = new ApiController();
            var result = await controller.ListOrdersAsync(repository, new EnforcementOrderSpec(), 1, 100);

            result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => e.GetIsPublic));
            result.Items.Cast<EnforcementOrderDetailedView>().Should().HaveCount(GetEnforcementOrders.Count(e => e.GetIsPublic));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                GetEnforcementOrderSummaryView(GetEnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => e.GetIsPublic).Id));
        }

        [Fact]
        public async Task GetOrder_UnknownId_Returns404Object()
        {
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(null as EnforcementOrderDetailedView);

            var controller = new ApiController();
            var response = await controller.GetOrderAsync(repo.Object, 1);

            response.Result.ShouldBeType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.ShouldEqual(404);
        }

        [Fact]
        public async Task GetOrder_ReturnsItem()
        {
            var itemId = GetEnforcementOrders.First().Id;
            var item = GetEnforcementOrderDetailedView(itemId);
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.GetAsync(itemId)).ReturnsAsync(item);

            var controller = new ApiController();
            var response = await controller.GetOrderAsync(repo.Object, itemId);

            response.Result.ShouldBeType<OkObjectResult>();
            var result = response.Result as OkObjectResult;

            result.ShouldNotBeNull();
            result?.Value.ShouldEqual(item);
        }

        [Fact]
        public async Task ListAuthorities_ReturnsActiveItems()
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();

            var controller = new ApiController();
            var response = await controller.ListLegalAuthoritiesAsync(repository);
            response.Should().HaveCount(GetLegalAuthorities.Count(e => e.Active));
        }

        [Fact]
        public async Task ListAuthorities_WithInactive_ReturnsAllItems()
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();

            var controller = new ApiController();
            var response = await controller.ListLegalAuthoritiesAsync(repository, true);
            response.Should().HaveCount(GetLegalAuthorities.Count());
        }

        [Fact]
        public async Task GetAuthority_UnknownId_Returns404Object()
        {
            var repo = new Mock<ILegalAuthorityRepository>();
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(null as LegalAuthorityView);

            var controller = new ApiController();
            var response = await controller.GetLegalAuthorityAsync(repo.Object, 1);

            response.Result.ShouldBeType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.ShouldEqual(404);
        }

        [Fact]
        public async Task GetAuthority_ReturnsItem()
        {
            var item = GetLegalAuthorityViewList().First();
            var repo = new Mock<ILegalAuthorityRepository>();
            repo.Setup(l => l.GetAsync(item.Id)).ReturnsAsync(item);

            var controller = new ApiController();
            var response = await controller.GetLegalAuthorityAsync(repo.Object, item.Id);

            response.Result.ShouldBeType<OkObjectResult>();
            var result = response.Result as OkObjectResult;

            result.ShouldNotBeNull();
            result?.Value.ShouldEqual(item);
        }
    }
}