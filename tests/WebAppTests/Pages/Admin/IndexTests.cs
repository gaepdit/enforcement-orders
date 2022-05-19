using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin;
using Enfo.WebApp.Platform.RazorHelpers;
using EnfoTests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin;

[TestFixture]
public class IndexTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrders()
    {
        var list = ResourceHelper.GetEnforcementOrderDetailedViewList();
        var adminList = ResourceHelper.GetEnforcementOrderAdminSummaryViewList();
        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.ListCurrentProposedEnforcementOrdersAsync())
            .ReturnsAsync(list);
        repo.Setup(l => l.ListRecentlyExecutedEnforcementOrdersAsync())
            .ReturnsAsync(list);
        repo.Setup(l => l.ListPendingEnforcementOrdersAsync())
            .ReturnsAsync(adminList);
        repo.Setup(l => l.ListDraftEnforcementOrdersAsync())
            .ReturnsAsync(adminList);
        var page = new Index(repo.Object);

        await page.OnGetAsync();

        Assert.Multiple(() =>
        {
            page.CurrentProposedOrders.Should().BeEquivalentTo(list);
            page.RecentExecutedOrders.Should().BeEquivalentTo(list);
            page.PendingOrders.Should().BeEquivalentTo(adminList);
            page.DraftOrders.Should().BeEquivalentTo(adminList);
            page.Message.Should().BeNull();
        });
    }

    [Test]
    public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
    {
        var repo = new Mock<IEnforcementOrderRepository> { DefaultValue = DefaultValue.Mock };
        var page = new Index(repo.Object);

        await page.OnGetAsync();

        Assert.Multiple(() =>
        {
            page.CurrentProposedOrders.Should().BeEmpty();
            page.RecentExecutedOrders.Should().BeEmpty();
            page.PendingOrders.Should().BeEmpty();
            page.DraftOrders.Should().BeEmpty();
            page.Message.Should().BeNull();
        });
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Initialize Page TempData
        var repo = new Mock<IEnforcementOrderRepository> { DefaultValue = DefaultValue.Mock };
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Index(repo.Object) { TempData = tempData };

        page.TempData.SetDisplayMessage(Context.Info, "Info message");
        await page.OnGetAsync();

        var expected = new DisplayMessage(Context.Info, "Info message");
        page.Message.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Find_GivenExists_ReturnsRedirectToDetails()
    {
        var list = ResourceHelper.GetEnforcementOrderAdminSummaryViewListOfOne();
        var listResult = new PaginatedResult<EnforcementOrderAdminSummaryView>(
            list, 1, new PaginationSpec(1, 1));

        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.ListAdminAsync(It.IsAny<EnforcementOrderAdminSpec>(), It.IsAny<PaginationSpec>()))
            .ReturnsAsync(listResult);
        var page = new Index(repo.Object);

        var result = await page.OnGetFindAsync("abc");

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues.Should().Contain(
                new KeyValuePair<string, object>("Id", list[0].Id));
        });
    }

    [Test]
    public async Task Find_GivenNotExists_ReturnsRedirectToSearch()
    {
        var list = ResourceHelper.GetEnforcementOrderAdminSummaryViewListOfOne();
        var listResult = new PaginatedResult<EnforcementOrderAdminSummaryView>(
            list, 2, new PaginationSpec(1, 1));

        var repo = new Mock<IEnforcementOrderRepository>();
        repo.Setup(l => l.ListAdminAsync(It.IsAny<EnforcementOrderAdminSpec>(), It.IsAny<PaginationSpec>()))
            .ReturnsAsync(listResult);
        var page = new Index(repo.Object);

        var result = await page.OnGetFindAsync("abc");

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("/Admin/Search");
            ((RedirectToPageResult)result).PageHandler.Should().Be("search");
            ((RedirectToPageResult)result).RouteValues.Should().Contain(
                new KeyValuePair<string, object>("OrderNumber", "abc"));
        });
    }

    [Test]
    public async Task Find_GivenEmptySearch_ReturnsPage()
    {
        var repo = new Mock<IEnforcementOrderRepository>();
        var page = new Index(repo.Object);

        var result = await page.OnGetFindAsync(string.Empty);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }
}
