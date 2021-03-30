using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.WebApp.Pages;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static TestHelpers.ResourceHelper;

namespace WebApp.Tests.Pages
{
    public class CurrentProposedTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithOrders()
        {
            var list = GetEnforcementOrderDetailedViewList();
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.ListCurrentProposedEnforcementOrdersAsync())
                .ReturnsAsync(list);
            var page = new CurrentProposed(repo.Object);

            await page.OnGetAsync();

            page.Orders.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
        {
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.ListCurrentProposedEnforcementOrdersAsync())
                .ReturnsAsync((IReadOnlyList<EnforcementOrderDetailedView>) null);
            var page = new CurrentProposed(repo.Object);

            await page.OnGetAsync();

            page.Orders.ShouldBeNull();
        }
    }
}