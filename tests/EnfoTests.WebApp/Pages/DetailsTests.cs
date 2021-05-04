using System.Linq;
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
using static EnfoTests.Helpers.DataHelper;

namespace EnfoTests.WebApp.Pages
{
    public class
        DetailsTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithOrder()
        {
            // Initialize Page ViewData
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext) {ViewData = viewData};

            var itemId = GetEnforcementOrders.First().Id;
            var item = GetEnforcementOrderDetailedView(itemId);
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.GetAsync(itemId)).ReturnsAsync(item);
            var page = new Details {PageContext = pageContext};

            await page.OnGetAsync(repo.Object, itemId);

            page.Item.ShouldEqual(item);
        }

        [Fact]
        public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
        {
            // Not testing returned Item, but it must be populated to return Page
            var itemId = GetEnforcementOrders.First().Id;
            var item = GetEnforcementOrderDetailedView(itemId);
            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.GetAsync(itemId)).ReturnsAsync(item);

            // Initialize Page TempData
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            // Initialize Page ViewData
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext) {ViewData = viewData};

            var page = new Details {TempData = tempData, PageContext = pageContext};

            page.TempData.SetDisplayMessage(Context.Info, "Info message");
            await page.OnGetAsync(repo.Object, itemId);

            var expected = new DisplayMessage(Context.Info, "Info message");
            page.Message.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task OnGet_NullId_ReturnsNotFound()
        {
            // Initialize Page ViewData
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext) {ViewData = viewData};

            var repo = new Mock<IEnforcementOrderRepository>();
            var page = new Details {PageContext = pageContext};

            var result = await page.OnGetAsync(repo.Object, null);

            result.Should().BeOfType<NotFoundResult>();
            page.Item.ShouldBeNull();
            page.Message.ShouldBeNull();
        }

        [Fact]
        public async Task OnGet_NonexistentId_ReturnsNotFound()
        {
            // Initialize Page ViewData
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext) {ViewData = viewData};

            var repo = new Mock<IEnforcementOrderRepository>();
            var page = new Details() {PageContext = pageContext};

            var result = await page.OnGetAsync(repo.Object, -1);

            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.ShouldBeNull();
            page.Message.ShouldBeNull();
        }
    }
}