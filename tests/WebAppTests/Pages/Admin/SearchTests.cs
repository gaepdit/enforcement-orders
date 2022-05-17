using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Pages.Admin;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestData;

namespace EnfoTests.WebApp.Pages.Admin;

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
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderAdminSpec());
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(),
                nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeFalse();
        });
    }

    [Test]
    public async Task OnGetSearch_ReturnsWithOrders()
    {
        var expectedOrders = new PaginatedResult<EnforcementOrderAdminSummaryView>(
            ResourceHelper.GetEnforcementOrderAdminSummaryViewListOfOne(), 1, new PaginationSpec(1, 1));
        var orderRepo = new Mock<IEnforcementOrderRepository>();
        orderRepo.Setup(l => l.ListAdminAsync(It.IsAny<EnforcementOrderAdminSpec>(), It.IsAny<PaginationSpec>()))
            .ReturnsAsync(expectedOrders);
        var legalRepo = new Mock<ILegalAuthorityRepository>();
        legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(ResourceHelper.GetLegalAuthorityViewList());
        var page = new Search(orderRepo.Object, legalRepo.Object);

        await page.OnGetSearchAsync(new EnforcementOrderAdminSpec());

        Assert.Multiple(() =>
        {
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderAdminSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(),
                nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeTrue();
        });
    }

    [Test]
    public async Task OnGetSearch_GivenNoResults_ReturnsWithEmptyOrders()
    {
        var expectedOrders = new PaginatedResult<EnforcementOrderAdminSummaryView>(
            new List<EnforcementOrderAdminSummaryView>(), 0, new PaginationSpec(1, 1));
        var orderRepo = new Mock<IEnforcementOrderRepository>();
        orderRepo.Setup(l => l.ListAdminAsync(It.IsAny<EnforcementOrderAdminSpec>(), It.IsAny<PaginationSpec>()))
            .ReturnsAsync(expectedOrders);
        var legalRepo = new Mock<ILegalAuthorityRepository>();
        legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(ResourceHelper.GetLegalAuthorityViewList());
        var page = new Search(orderRepo.Object, legalRepo.Object);

        await page.OnGetSearchAsync(new EnforcementOrderAdminSpec());

        Assert.Multiple(() =>
        {
            page.Spec.Should().BeEquivalentTo(new EnforcementOrderAdminSpec());
            page.OrdersList.Should().BeEquivalentTo(expectedOrders);
            var expectedLegal = new SelectList(ResourceHelper.GetLegalAuthorityViewList(),
                nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
            page.LegalAuthoritiesSelectList.Should().BeEquivalentTo(expectedLegal);
            page.ShowResults.Should().BeTrue();
        });
    }
}
