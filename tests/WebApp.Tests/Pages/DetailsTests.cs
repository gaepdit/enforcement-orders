using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages;
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
using static TestHelpers.ResourceHelper;
using static TestHelpers.DataHelper;

namespace WebApp.Tests.Pages
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
            var page = new Details(repo.Object) {PageContext = pageContext};

            await page.OnGetAsync(itemId);

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

            var page = new Details(repo.Object) {TempData = tempData, PageContext = pageContext};

            page.TempData.SetDisplayMessage(Context.Info, "Info message");
            await page.OnGetAsync(itemId);

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

            var mockRepo = new Mock<IEnforcementOrderRepository>();
            var page = new Details(mockRepo.Object) {PageContext = pageContext};

            var result = await page.OnGetAsync(null);

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

            var mockRepo = new Mock<IEnforcementOrderRepository>();
            var page = new Details(mockRepo.Object) {PageContext = pageContext};

            var result = await page.OnGetAsync(-1);

            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.ShouldBeNull();
            page.Message.ShouldBeNull();
        }
    }
}