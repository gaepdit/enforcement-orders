using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests.Pages;

[TestFixture]
public class DetailsTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrder()
    {
        var itemId = EnforcementOrderData.EnforcementOrders[0].Id;
        var item = ResourceHelper.GetEnforcementOrderDetailedView(itemId);
        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAsync(itemId).Returns(item);
        var page = new Details();

        await page.OnGetAsync(repo, itemId);

        page.Item.Should().Be(item);
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Not testing returned Item, but it must be populated to return Page
        var itemId = EnforcementOrderData.EnforcementOrders[0].Id;
        var item = ResourceHelper.GetEnforcementOrderDetailedView(itemId);
        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.GetAsync(itemId).Returns(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());

        var page = new Details { TempData = tempData };

        page.TempData.SetDisplayMessage(Context.Info, "Info message");
        await page.OnGetAsync(repo, itemId);

        var expected = new DisplayMessage(Context.Info, "Info message");
        page.Message.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task OnGet_NullId_ReturnsNotFound()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var page = new Details();

        var result = await page.OnGetAsync(repo, null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
            page.Message.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_NonexistentId_ReturnsNotFound()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var page = new Details();

        var result = await page.OnGetAsync(repo, -1);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
            page.Message.Should().BeNull();
        }
    }
}
