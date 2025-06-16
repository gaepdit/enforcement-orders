using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Index = Enfo.WebApp.Pages.Index;

namespace EnfoTests.WebApp.Pages;

[TestFixture]
public class IndexTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrders()
    {
        var list = ResourceHelper.GetEnforcementOrderDetailedViewList();
        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.ListCurrentProposedEnforcementOrdersAsync().Returns(list);
        repo.ListRecentlyExecutedEnforcementOrdersAsync().Returns(list);
        var page = new Index();

        await page.OnGetAsync(repo);

        using (new AssertionScope())
        {
            page.CurrentProposedOrders.Should().BeEquivalentTo(list);
            page.RecentExecutedOrders.Should().BeEquivalentTo(list);
            page.Message.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var page = new Index();

        await page.OnGetAsync(repo);

        using (new AssertionScope())
        {
            page.CurrentProposedOrders.Should().BeEmpty();
            page.RecentExecutedOrders.Should().BeEmpty();
            page.Message.Should().BeNull();
        }
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Initialize Page TempData
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());

        var page = new Index { TempData = tempData };

        page.TempData.SetDisplayMessage(Context.Info, "Info message");
        await page.OnGetAsync(repo);

        var expected = new DisplayMessage(Context.Info, "Info message");
        page.Message.Should().BeEquivalentTo(expected);
    }
}
