using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.WebApp.Pages.Admin;

[TestFixture]
public class AddTests
{
    [Test]
    public async Task OnGet_ReturnsWithDefaultCreateResource()
    {
        var legalRepo = new Mock<ILegalAuthorityRepository>();
        legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<LegalAuthorityView>());
        var contactRepo = new Mock<IEpdContactRepository>();
        contactRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<EpdContactView>());
        var page = new Add(Mock.Of<IEnforcementOrderRepository>(), legalRepo.Object, contactRepo.Object);

        await page.OnGetAsync();
        page.Item.Should().BeEquivalentTo(new EnforcementOrderCreate());
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var item = GetValidEnforcementOrderCreate();
        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        // Mock repos
        var orderRepo = new Mock<IEnforcementOrderRepository>();
        orderRepo.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(false);
        orderRepo.Setup(l => l.CreateAsync(item)).ReturnsAsync(9);
        // Construct Page
        var page = new Add(orderRepo.Object, Mock.Of<ILegalAuthorityRepository>(), Mock.Of<IEpdContactRepository>())
            { TempData = tempData, Item = item };

        var result = await page.OnPostAsync();

        var expected = new DisplayMessage(Context.Success,
            "The new Enforcement Order has been successfully added.");

        Assert.Multiple(() =>
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(9);
        });
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var item = GetValidEnforcementOrderCreate();
        // Mock repos
        var legalRepo = new Mock<ILegalAuthorityRepository>();
        legalRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<LegalAuthorityView>());
        var contactRepo = new Mock<IEpdContactRepository>();
        contactRepo.Setup(l => l.ListAsync(false)).ReturnsAsync(new List<EpdContactView>());
        var orderRepo = new Mock<IEnforcementOrderRepository>();
        orderRepo.Setup(l => l.OrderNumberExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(false);
        // Construct Page
        var page = new Add(orderRepo.Object, legalRepo.Object, contactRepo.Object)
            { Item = item };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync();

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.ModelState.ErrorCount.Should().Be(1);
        });
    }
}
