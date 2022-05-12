using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Api;

public class ApiTests
{
    [Fact]
    public async Task ListOrders_ReturnsPublicItems()
    {
        const string baseUrl = "https://localhost";
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "BaseUrl", baseUrl } })
            .Build();

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();

        var controller = new ApiController();
        var result = await controller.ListOrdersAsync(repository, config, new EnforcementOrderSpec(), 1, 100);

        result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => e.GetIsPublic));
        result.Items.Should()
            .HaveCount(GetEnforcementOrders.Count(e => e.GetIsPublic));
        result.PageNumber.Should().Be(1);
        var order = result.Items[0].EnforcementOrder; 
        order.Should().BeEquivalentTo(
            GetEnforcementOrderSummaryView(GetEnforcementOrders
                .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                .ThenBy(e => e.FacilityName)
                .First(e => e.GetIsPublic).Id));
        result.Items[0].Link.ShouldEqual($"{baseUrl}/Details/{order.Id}");
    }

    [Fact]
    public async Task GetOrder_UnknownId_Returns404Object()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "BaseUrl", string.Empty } })
            .Build();

        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(null as EnforcementOrderDetailedView);

        var controller = new ApiController();
        var response = await controller.GetOrderAsync(repo.Object, config, 1);

        response.Result.ShouldBeType<ObjectResult>();
        var result = response.Result as ObjectResult;
        result?.StatusCode.ShouldEqual(404);
    }

    [Fact]
    public async Task GetOrder_ReturnsItem()
    {
        const string baseUrl = "https://localhost";
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "BaseUrl", baseUrl } })
            .Build();

        var itemId = GetEnforcementOrders.First().Id;
        var item = GetEnforcementOrderDetailedView(itemId);
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAsync(itemId)).ReturnsAsync(item);

        var controller = new ApiController();
        var response = await controller.GetOrderAsync(repo.Object, config, itemId);

        response.Result.ShouldBeType<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result.ShouldNotBeNull();
        var resultValue = (EnforcementOrderApiView)result?.Value;
        resultValue!.EnforcementOrder.ShouldEqual(item);
        resultValue.Link.ShouldEqual($"{baseUrl}/Details/{itemId}");
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
