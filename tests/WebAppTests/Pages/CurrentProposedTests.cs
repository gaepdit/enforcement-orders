using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.WebApp.Pages;
using EnfoTests.TestData;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages;

[TestFixture]
public class CurrentProposedTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrders()
    {
        var list = ResourceHelper.GetEnforcementOrderDetailedViewList();
        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.ListCurrentProposedEnforcementOrdersAsync().Returns(list);
        var page = new CurrentProposed();

        await page.OnGetAsync(repo);

        page.Orders.Should().BeEquivalentTo(list);
    }

    [Test]
    public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
    {
        var repo = Substitute.For<IEnforcementOrderRepository>();
        repo.ListCurrentProposedEnforcementOrdersAsync().Returns(null as IReadOnlyList<EnforcementOrderDetailedView>);
        var page = new CurrentProposed();

        await page.OnGetAsync(repo);

        page.Orders.Should().BeNull();
    }
}
