using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.WebApp.Pages;

namespace WebAppTests.Pages;

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
        var page = new IndexModel();

        await page.OnGetAsync(repo);

        using (new AssertionScope())
        {
            page.CurrentProposedOrders.Should().BeEquivalentTo(list);
            page.RecentExecutedOrders.Should().BeEquivalentTo(list);
        }
    }

    [Test]
    public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        var page = new IndexModel();

        await page.OnGetAsync(repo);

        using (new AssertionScope())
        {
            page.CurrentProposedOrders.Should().BeEmpty();
            page.RecentExecutedOrders.Should().BeEmpty();
        }
    }
}
