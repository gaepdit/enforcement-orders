using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.WebApp.Pages;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages;

[TestFixture]
public class RecentExecutedTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrders()
    {
        var list = GetEnforcementOrderDetailedViewList();
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.ListRecentlyExecutedEnforcementOrdersAsync())
            .ReturnsAsync(list);
        var page = new RecentExecuted();

        await page.OnGetAsync(repo.Object);

        page.Orders.Should().BeEquivalentTo(list);
    }

    [Test]
    public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
    {
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.ListRecentlyExecutedEnforcementOrdersAsync())
            .ReturnsAsync(null as IReadOnlyList<EnforcementOrderDetailedView>);
        var page = new RecentExecuted();

        await page.OnGetAsync(repo.Object);

        page.Orders.Should().BeNull();
    }
}
