using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Specs;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin;
using Enfo.WebApp.Platform.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static TestHelpers.ResourceHelper;

namespace WebApp.Tests.Pages.Admin
{
    public class IndexTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithOrders()
        {
            var list = GetEnforcementOrderDetailedViewList();
            var adminList = GetEnforcementOrderAdminSummaryViewList();
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

            page.CurrentProposedOrders.Should().BeEquivalentTo(list);
            page.RecentExecutedOrders.Should().BeEquivalentTo(list);
            page.PendingOrders.Should().BeEquivalentTo(adminList);
            page.DraftOrders.Should().BeEquivalentTo(adminList);
            page.Message.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
        {
            var repo = new Mock<IEnforcementOrderRepository> {DefaultValue = DefaultValue.Mock};
            var page = new Index(repo.Object);

            await page.OnGetAsync();

            page.CurrentProposedOrders.ShouldBeEmpty();
            page.RecentExecutedOrders.ShouldBeEmpty();
            page.PendingOrders.ShouldBeEmpty();
            page.DraftOrders.ShouldBeEmpty();
            page.Message.ShouldBeNull();
        }

        [Fact]
        public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
        {
            // Initialize Page TempData
            var repo = new Mock<IEnforcementOrderRepository> {DefaultValue = DefaultValue.Mock};
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var page = new Index(repo.Object) {TempData = tempData};

            page.TempData.SetDisplayMessage(Context.Info, "Info message");
            await page.OnGetAsync();

            var expected = new DisplayMessage(Context.Info, "Info message");
            page.Message.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Find_GivenExists_ReturnsRedirectToDetails()
        {
            var list = GetEnforcementOrderAdminSummaryViewListOfOne();
            var listResult = new PaginatedResult<EnforcementOrderAdminSummaryView>(
                list, 1, new PaginationSpec(1, 1));

            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.ListAdminAsync(It.IsAny<EnforcementOrderAdminSpec>(), It.IsAny<PaginationSpec>()))
                .ReturnsAsync(listResult);
            var page = new Index(repo.Object);

            var result = await page.OnGetFindAsync("abc");

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Details");
            ((RedirectToPageResult) result).RouteValues.ShouldContain(
                new KeyValuePair<string, object>("Id", list[0].Id));
        }

        [Fact]
        public async Task Find_GivenNotExists_ReturnsRedirectToSearch()
        {
            var list = GetEnforcementOrderAdminSummaryViewListOfOne();
            var listResult = new PaginatedResult<EnforcementOrderAdminSummaryView>(
                list, 2, new PaginationSpec(1, 1));

            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.ListAdminAsync(It.IsAny<EnforcementOrderAdminSpec>(), It.IsAny<PaginationSpec>()))
                .ReturnsAsync(listResult);
            var page = new Index(repo.Object);

            var result = await page.OnGetFindAsync("abc");

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("/Admin/Search");
            ((RedirectToPageResult) result).PageHandler.ShouldEqual("search");
            ((RedirectToPageResult) result).RouteValues.ShouldContain(
                new KeyValuePair<string, object>("OrderNumber", "abc"));
        }

        [Fact]
        public async Task Find_GivenEmptySearch_ReturnsPage()
        {
            var repo = new Mock<IEnforcementOrderRepository>();
            var page = new Index(repo.Object);

            var result = await page.OnGetFindAsync(string.Empty);

            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult) result).PageName.ShouldEqual("Index");
        }
    }
}