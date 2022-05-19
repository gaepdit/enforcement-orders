using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Services;
using Enfo.LocalRepository;
using Enfo.WebApp.Api;
using EnfoTests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Api;

[TestFixture]
public class ApiTests
{
    [Test]
    public async Task ListOrders_ReturnsPublicItems()
    {
        const string baseUrl = "https://localhost";
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "BaseUrl", baseUrl } })
            .Build();

        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var controller = new ApiController();
        var result = await controller.ListOrdersAsync(repository, config, new EnforcementOrderSpec(), 1, 100);

        Assert.Multiple(() =>
        {
            result.TotalCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.Items.Should()
                .HaveCount(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.PageNumber.Should().Be(1);
            var order = result.Items[0].EnforcementOrder;
            order.Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => e.GetIsPublic).Id));
            result.Items[0].Link.Should().Be($"{baseUrl}/Details/{order.Id}");
        });
    }

    [Test]
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

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.Should().Be(404);
        });
    }

    [Test]
    public async Task GetOrder_ReturnsItem()
    {
        const string baseUrl = "https://localhost";
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "BaseUrl", baseUrl } })
            .Build();

        var itemId = EnforcementOrderData.EnforcementOrders.First().Id;
        var item = ResourceHelper.GetEnforcementOrderDetailedView(itemId);
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAsync(itemId)).ReturnsAsync(item);

        var controller = new ApiController();
        var response = await controller.GetOrderAsync(repo.Object, config, itemId);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;

            result.Should().NotBeNull();
            var resultValue = (EnforcementOrderApiView)result?.Value;
            resultValue!.EnforcementOrder.Should().Be(item);
            resultValue.Link.Should().Be($"{baseUrl}/Details/{itemId}");
        });
    }

    [Test]
    public async Task ListAuthorities_ReturnsActiveItems()
    {
        using var repository = new LegalAuthorityRepository();

        var controller = new ApiController();
        var response = await controller.ListLegalAuthoritiesAsync(repository);
        response.Should().HaveCount(LegalAuthorityData.LegalAuthorities.Count(e => e.Active));
    }

    [Test]
    public async Task ListAuthorities_WithInactive_ReturnsAllItems()
    {
        using var repository = new LegalAuthorityRepository();

        var controller = new ApiController();
        var response = await controller.ListLegalAuthoritiesAsync(repository, true);
        response.Should().HaveCount(LegalAuthorityData.LegalAuthorities.Count);
    }

    [Test]
    public async Task GetAuthority_UnknownId_Returns404Object()
    {
        var repo = new Mock<ILegalAuthorityRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(null as LegalAuthorityView);

        var controller = new ApiController();
        var response = await controller.GetLegalAuthorityAsync(repo.Object, 1);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.Should().Be(404);
        });
    }

    [Test]
    public async Task GetAuthority_ReturnsItem()
    {
        var item = ResourceHelper.GetLegalAuthorityViewList().First();
        var repo = new Mock<ILegalAuthorityRepository>();
        repo.Setup(l => l.GetAsync(item.Id)).ReturnsAsync(item);

        var controller = new ApiController();
        var response = await controller.GetLegalAuthorityAsync(repo.Object, item.Id);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;

            result.Should().NotBeNull();
            result?.Value.Should().Be(item);
        });
    }
}
