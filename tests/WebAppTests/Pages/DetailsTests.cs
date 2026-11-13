using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.WebApp.Pages;
using Microsoft.AspNetCore.Mvc;

namespace WebAppTests.Pages;

[TestFixture]
public class DetailsModelTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrder()
    {
        var itemId = EnforcementOrderData.EnforcementOrders[0].Id;
        var item = ResourceHelper.GetEnforcementOrderDetailedView(itemId);
        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAsync(itemId).Returns(item);
        var page = new DetailsModel { Id = itemId };

        await page.OnGetAsync(repo);

        page.Item.Should().Be(item);
    }

    [Test]
    public async Task OnGet_NullId_ReturnsNotFound()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var page = new DetailsModel { Id = 0 };

        var result = await page.OnGetAsync(repo);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_NonexistentId_ReturnsNotFound()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var page = new DetailsModel() { Id = -1 };

        var result = await page.OnGetAsync(repo);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
        }
    }
}
