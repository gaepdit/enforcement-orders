using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Pages;
using EnfoTests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages;

[TestFixture]
public class SearchTests
{
    [Test]
    public async Task OnGet_ReturnsWithDefaults()
    {
        var orderRepo = new Mock<IEnforcementOrderRepository>();
        var legalRepo = new Mock<ILegalAuthorityRepository>();
        legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(ResourceHelper.GetLegalAuthorityViewList());
        var page = new Search(orderRepo.Object, legalRepo.Object);

        await page.OnGetAsync();

        Assert.Multiple(() =>
        {
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeFalse();
        });
    }

    [Test]
    public async Task OnGetSearch_ReturnsWithOrders()
    {
        var expectedOrders = new PaginatedResult<EnforcementOrderSummaryView>(
            ResourceHelper.GetEnforcementOrderSummaryViewListOfOne(), 1, new PaginationSpec(1, 1));
        var orderRepo = new Mock<IEnforcementOrderRepository>();
        orderRepo.Setup(l => l.ListAsync(It.IsAny<EnforcementOrderSpec>(), It.IsAny<PaginationSpec>()))
            .ReturnsAsync(expectedOrders);
        var legalRepo = new Mock<ILegalAuthorityRepository>();
        legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(ResourceHelper.GetLegalAuthorityViewList());
        var page = new Search(orderRepo.Object, legalRepo.Object);

        await page.OnGetSearchAsync(new EnforcementOrderSpec());

        Assert.Multiple(() =>
        {
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeTrue();
        });
    }

    [Test]
    public async Task OnGetSearch_GivenNoResults_ReturnsWithEmptyOrders()
    {
        var expectedOrders = new PaginatedResult<EnforcementOrderSummaryView>(
            new List<EnforcementOrderSummaryView>(), 0, new PaginationSpec(1, 1));
        var orderRepo = new Mock<IEnforcementOrderRepository>();
        orderRepo.Setup(l => l.ListAsync(It.IsAny<EnforcementOrderSpec>(), It.IsAny<PaginationSpec>()))
            .ReturnsAsync(expectedOrders);
        var legalRepo = new Mock<ILegalAuthorityRepository>();
        legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(ResourceHelper.GetLegalAuthorityViewList());
        var page = new Search(orderRepo.Object, legalRepo.Object);

        await page.OnGetSearchAsync(new EnforcementOrderSpec());

        Assert.Multiple(() =>
        {
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(), nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeTrue();
        });
    }
}
