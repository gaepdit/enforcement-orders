using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.WebApp.Pages;
using FluentAssertions;
using Moq;
using Xunit;
using static TestHelpers.ResourceHelper;

namespace WebApp.Tests
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
    }
}