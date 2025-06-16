using Enfo.Domain.Attachments;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.LocalRepository.Repositories;
using Enfo.WebApp.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebAppTests.Api;

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

        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var controller = new ApiController();
        var result = await controller.ListOrdersAsync(repository, config, new EnforcementOrderSpec(), 1, 100);

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.Items.Should()
                .HaveCount(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.PageNumber.Should().Be(1);
            var order = result.Items[0];
            order.Should().BeEquivalentTo(
                new EnforcementOrderApiView(
                    ResourceHelper.GetEnforcementOrderDetailedView(EnforcementOrderData.EnforcementOrders
                        .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                        .ThenBy(e => e.FacilityName)
                        .First(e => e.GetIsPublic).Id), baseUrl));
            result.Items[0].Link.Should().Be($"{baseUrl}/Details/{order.Id}");
        }
    }

    [Test]
    public async Task GetOrder_UnknownId_Returns404Object()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "BaseUrl", string.Empty } })
            .Build();

        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(null as EnforcementOrderDetailedView);

        var controller = new ApiController();
        var response = await controller.GetOrderAsync(repo, config, 1);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.Should().Be(404);
        }
    }

    [Test]
    public async Task GetOrder_ReturnsItem()
    {
        const string baseUrl = "https://localhost";
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "BaseUrl", baseUrl } })
            .Build();

        var itemId = EnforcementOrderData.EnforcementOrders[0].Id;
        var item = ResourceHelper.GetEnforcementOrderDetailedView(itemId);
        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAsync(itemId).Returns(item);

        var controller = new ApiController();
        var response = await controller.GetOrderAsync(repo, config, itemId);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;

            result.Should().NotBeNull();
            var resultValue = (EnforcementOrderApiView)result?.Value;
            resultValue.Should().BeEquivalentTo(new EnforcementOrderApiView(item, baseUrl));
            resultValue!.Link.Should().Be($"{baseUrl}/Details/{itemId}");
        }
    }

    [Test]
    public async Task ListAuthorities_ReturnsActiveItems()
    {
        using var repository = new LocalLegalAuthorityRepository();

        var controller = new ApiController();
        var response = await controller.ListLegalAuthoritiesAsync(repository);
        response.Should().HaveCount(LegalAuthorityData.LegalAuthorities.Count(e => e.Active));
    }

    [Test]
    public async Task ListAuthorities_WithInactive_ReturnsAllItems()
    {
        using var repository = new LocalLegalAuthorityRepository();

        var controller = new ApiController();
        var response = await controller.ListLegalAuthoritiesAsync(repository, true);
        response.Should().HaveSameCount(LegalAuthorityData.LegalAuthorities);
    }

    [Test]
    public async Task GetAuthority_UnknownId_Returns404Object()
    {
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(null as LegalAuthorityView);

        var controller = new ApiController();
        var response = await controller.GetLegalAuthorityAsync(repo, 1);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.Should().Be(404);
        }
    }

    [Test]
    public async Task GetAuthority_ReturnsItem()
    {
        var item = ResourceHelper.GetLegalAuthorityViewList()[0];
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(item.Id).Returns(item);

        var controller = new ApiController();
        var response = await controller.GetLegalAuthorityAsync(repo, item.Id);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;

            result.Should().NotBeNull();
            result?.Value.Should().Be(item);
        }
    }
}
