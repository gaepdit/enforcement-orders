using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnfoTests.WebApp.Pages;

[TestFixture]
public class SearchTests
{
    [Test]
    public async Task OnGet_ReturnsWithDefaults()
    {
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        var legalRepo = Substitute.For<ILegalAuthorityRepository>();
        legalRepo.ListAsync().Returns(ResourceHelper.GetLegalAuthorityViewList());
        var page = new Search(orderRepo, legalRepo);

        await page.OnGetAsync();

        using (new AssertionScope())
        {
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(),
                nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeFalse();
        }
    }

    [Test]
    public async Task OnGetSearch_ReturnsWithOrders()
    {
        var expectedOrders = new PaginatedResult<EnforcementOrderSummaryView>(
            ResourceHelper.GetEnforcementOrderSummaryViewListOfOne(), 1, new PaginationSpec(1, 1));
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.ListAsync(Arg.Any<EnforcementOrderSpec>(), Arg.Any<PaginationSpec>()).Returns(expectedOrders);
        var legalRepo = Substitute.For<ILegalAuthorityRepository>();
        legalRepo.ListAsync().Returns(ResourceHelper.GetLegalAuthorityViewList());
        var page = new Search(orderRepo, legalRepo);

        await page.OnGetSearchAsync(new EnforcementOrderSpec());

        using (new AssertionScope())
        {
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(),
                nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeTrue();
        }
    }

    [Test]
    public async Task OnGetSearch_GivenNoResults_ReturnsWithEmptyOrders()
    {
        var expectedOrders = new PaginatedResult<EnforcementOrderSummaryView>(
            new List<EnforcementOrderSummaryView>(), 0, new PaginationSpec(1, 1));
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.ListAsync(Arg.Any<EnforcementOrderSpec>(), Arg.Any<PaginationSpec>()).Returns(expectedOrders);
        var legalRepo = Substitute.For<ILegalAuthorityRepository>();
        legalRepo.ListAsync().Returns(ResourceHelper.GetLegalAuthorityViewList());
        var page = new Search(orderRepo, legalRepo);

        await page.OnGetSearchAsync(new EnforcementOrderSpec());

        using (new AssertionScope())
        {
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(),
                nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeTrue();
        }
    }
}
