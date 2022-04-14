using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.WebApp.Pages;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages
{
    public class RecentExecutedTests
    {
        [Fact]
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

        [Fact]
        public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
        {
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.ListRecentlyExecutedEnforcementOrdersAsync())
                .ReturnsAsync(null as IReadOnlyList<EnforcementOrderDetailedView>);
            var page = new RecentExecuted();

            await page.OnGetAsync(repo.Object);

            page.Orders.ShouldBeNull();
        }
    }
}