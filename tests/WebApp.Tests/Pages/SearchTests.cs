using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Specs;
using Enfo.WebApp.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static TestHelpers.ResourceHelper;

namespace WebApp.Tests.Pages
{
    public class SearchTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithDefaults()
        {
            // Initialize Page ViewData
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext) {ViewData = viewData};

            var orderRepo = new Mock<IEnforcementOrderRepository>();
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(GetLegalAuthorityViewList());
            var page = new Search(orderRepo.Object, legalRepo.Object) {PageContext = pageContext};

            await page.OnGetAsync();

            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            var expectedLegal = new SelectList(GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.ShouldBeFalse();
        }

        [Fact]
        public async Task OnGetSearch_ReturnsWithOrders()
        {
            var expectedOrders = new PaginatedResult<EnforcementOrderSummaryView>(
                GetEnforcementOrderSummaryViewListOfOne(), 1, new PaginationSpec(1, 1));
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.ListAsync(It.IsAny<EnforcementOrderSpec>(), It.IsAny<PaginationSpec>()))
                .ReturnsAsync(expectedOrders);
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(GetLegalAuthorityViewList());
            var page = new Search(orderRepo.Object, legalRepo.Object);

            await page.OnGetSearchAsync(new EnforcementOrderSpec());

            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.ShouldBeTrue();
        }

        [Fact]
        public async Task OnGetSearch_GivenNoResults_ReturnsWithEmptyOrders()
        {
            var expectedOrders = new PaginatedResult<EnforcementOrderSummaryView>(
                new List<EnforcementOrderSummaryView>(), 0, new PaginationSpec(1, 1));
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.ListAsync(It.IsAny<EnforcementOrderSpec>(), It.IsAny<PaginationSpec>()))
                .ReturnsAsync(expectedOrders);
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(GetLegalAuthorityViewList());
            var page = new Search(orderRepo.Object, legalRepo.Object);

            await page.OnGetSearchAsync(new EnforcementOrderSpec());

            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.ShouldBeTrue();
        }
    }
}