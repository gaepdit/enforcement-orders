using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages;
using Enfo.WebApp.Platform.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages
{
    public class IndexTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithOrders()
        {
            // Initialize Page ViewData
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext) {ViewData = viewData};

            var list = GetEnforcementOrderDetailedViewList();
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.ListCurrentProposedEnforcementOrdersAsync())
                .ReturnsAsync(list);
            repo.Setup(l => l.ListRecentlyExecutedEnforcementOrdersAsync())
                .ReturnsAsync(list);
            var page = new Index {PageContext = pageContext};

            await page.OnGetAsync(repo.Object);

            page.CurrentProposedOrders.Should().BeEquivalentTo(list);
            page.RecentExecutedOrders.Should().BeEquivalentTo(list);
            page.Message.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_GivenNoResults_ReturnsWithEmptyOrders()
        {
            // Initialize Page ViewData
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext) {ViewData = viewData};

            var repo = new Mock<IEnforcementOrderRepository> {DefaultValue = DefaultValue.Mock};
            var page = new Index {PageContext = pageContext};

            await page.OnGetAsync(repo.Object);

            page.CurrentProposedOrders.ShouldBeEmpty();
            page.RecentExecutedOrders.ShouldBeEmpty();
            page.Message.ShouldBeNull();
        }

        [Fact]
        public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
        {
            // Initialize Page TempData
            var repo = new Mock<IEnforcementOrderRepository> {DefaultValue = DefaultValue.Mock};
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            // Initialize Page ViewData
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext) {ViewData = viewData};

            var page = new Index {TempData = tempData, PageContext = pageContext};

            page.TempData.SetDisplayMessage(Context.Info, "Info message");
            await page.OnGetAsync(repo.Object);

            var expected = new DisplayMessage(Context.Info, "Info message");
            page.Message.Should().BeEquivalentTo(expected);
        }
    }
}