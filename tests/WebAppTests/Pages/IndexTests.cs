using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages;
using Enfo.WebApp.Platform.RazorHelpers;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages;

[TestFixture]
public class IndexTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrders()
    {
        var list = ResourceHelper.GetEnforcementOrderDetailedViewList();
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.ListCurrentProposedEnforcementOrdersAsync())
            .ReturnsAsync(list);
        repo.Setup(l => l.ListRecentlyExecutedEnforcementOrdersAsync())
            .ReturnsAsync(list);
        var page = new Index();

        await page.OnGetAsync(repo.Object);

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
        var repo = new Mock<IEnforcementOrderRepository> { DefaultValue = DefaultValue.Mock };
        var page = new Index();

        await page.OnGetAsync(repo.Object);

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
        var repo = new Mock<IEnforcementOrderRepository> { DefaultValue = DefaultValue.Mock };
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

        var page = new Index { TempData = tempData };

        page.TempData.SetDisplayMessage(Context.Info, "Info message");
        await page.OnGetAsync(repo.Object);

        var expected = new DisplayMessage(Context.Info, "Info message");
        page.Message.Should().BeEquivalentTo(expected);
    }
}
