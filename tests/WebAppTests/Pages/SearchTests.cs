using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.WebApp.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages
{
    public class SearchTests
    {
        [Fact]
        public async Task OnGet_ReturnsWithDefaults()
        {
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(GetLegalAuthorityViewList());
            var page = new Search(orderRepo.Object, legalRepo.Object);

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