using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
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
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages
{
    public class RecentExecutedTests
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
            repo.Setup(l => l.ListRecentlyExecutedEnforcementOrdersAsync())
                .ReturnsAsync(list);
            var page = new RecentExecuted(repo.Object) {PageContext = pageContext};

            await page.OnGetAsync();

            page.Orders.Should().BeEquivalentTo(list);
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

            var repo = new Mock<IEnforcementOrderRepository>();
            repo.Setup(l => l.ListRecentlyExecutedEnforcementOrdersAsync())
                .ReturnsAsync(null as IReadOnlyList<EnforcementOrderDetailedView>);
            var page = new RecentExecuted(repo.Object) {PageContext = pageContext};

            await page.OnGetAsync();

            page.Orders.ShouldBeNull();
        }
    }
}