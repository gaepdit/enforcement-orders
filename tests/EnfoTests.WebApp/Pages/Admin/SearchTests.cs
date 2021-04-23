using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.Domain.Specs;
using Enfo.WebApp.Pages.Admin;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages.Admin
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

            page.Spec.Should().BeEquivalentTo(new EnforcementOrderAdminSpec());
            var expectedLegal = new SelectList(GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.ShouldBeFalse();
        }

        [Fact]
        public async Task OnGetSearch_ReturnsWithOrders()
        {
            var expectedOrders = new PaginatedResult<EnforcementOrderAdminSummaryView>(
                GetEnforcementOrderAdminSummaryViewListOfOne(), 1, new PaginationSpec(1, 1));
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.ListAdminAsync(It.IsAny<EnforcementOrderAdminSpec>(), It.IsAny<PaginationSpec>()))
                .ReturnsAsync(expectedOrders);
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(GetLegalAuthorityViewList());
            var page = new Search(orderRepo.Object, legalRepo.Object);

            await page.OnGetSearchAsync(new EnforcementOrderAdminSpec());

            page.Spec.Should().BeEquivalentTo(new EnforcementOrderAdminSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.ShouldBeTrue();
        }
        
        [Fact]
        public async Task OnGetSearch_GivenNoResults_ReturnsWithEmptyOrders()
        {
            var expectedOrders = new PaginatedResult<EnforcementOrderAdminSummaryView>(
                new List<EnforcementOrderAdminSummaryView>(), 0, new PaginationSpec(1, 1));
            var orderRepo = new Mock<IEnforcementOrderRepository>();
            orderRepo.Setup(l => l.ListAdminAsync(It.IsAny<EnforcementOrderAdminSpec>(), It.IsAny<PaginationSpec>()))
                .ReturnsAsync(expectedOrders);
            var legalRepo = new Mock<ILegalAuthorityRepository>();
            legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(GetLegalAuthorityViewList());
            var page = new Search(orderRepo.Object, legalRepo.Object);

            await page.OnGetSearchAsync(new EnforcementOrderAdminSpec());

            page.Spec.Should().BeEquivalentTo(new EnforcementOrderAdminSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.ShouldBeTrue();
        }
    }
}