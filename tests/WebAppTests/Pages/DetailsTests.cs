using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages;
using Enfo.WebApp.Platform.RazorHelpers;
using EnfoTests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages;

[TestFixture]
public class DetailsTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrder()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First().Id;
        var item = ResourceHelper.GetEnforcementOrderDetailedView(itemId);
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAsync(itemId)).ReturnsAsync(item);
        var page = new Details();

        await page.OnGetAsync(repo.Object, itemId);

        page.Item.Should().Be(item);
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Not testing returned Item, but it must be populated to return Page
        var itemId = EnforcementOrderData.EnforcementOrders.First().Id;
        var item = ResourceHelper.GetEnforcementOrderDetailedView(itemId);
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.GetAsync(itemId)).ReturnsAsync(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

        var page = new Details { TempData = tempData };

        page.TempData.SetDisplayMessage(Context.Info, "Info message");
        await page.OnGetAsync(repo.Object, itemId);

        var expected = new DisplayMessage(Context.Info, "Info message");
        page.Message.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task OnGet_NullId_ReturnsNotFound()
    {
        var repo = new Mock<IEnforcementOrderRepository>();
        var page = new Details();

        var result = await page.OnGetAsync(repo.Object, null);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
            page.Message.Should().BeNull();
        });
    }

    [Test]
    public async Task OnGet_NonexistentId_ReturnsNotFound()
    {
        var repo = new Mock<IEnforcementOrderRepository>();
        var page = new Details();

        var result = await page.OnGetAsync(repo.Object, -1);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
            page.Message.Should().BeNull();
        });
    }
}
