using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EnfoTests.WebApp.Pages.Admin;

[TestFixture]
public class EditTests
{
    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var item = ResourceHelper.GetEnforcementOrderAdminView(1);
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.GetAdminViewAsync(Arg.Any<int>()).Returns(item);
        var legalRepo = Substitute.For<ILegalAuthorityRepository>();
        legalRepo.ListAsync().Returns(new List<LegalAuthorityView>());
        var contactRepo = Substitute.For<IEpdContactRepository>();
        contactRepo.ListAsync().Returns(new List<EpdContactView>());
        var page = new Edit(orderRepo, legalRepo, contactRepo);

        await page.OnGetAsync(1);

        using (new AssertionScope())
        {
            page.Item.Should().BeEquivalentTo(new EnforcementOrderUpdate(item));
            page.Item.Id.Should().Be(item.Id);
            page.OriginalOrderNumber.Should().Be(item.OrderNumber);
        }
    }


    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var page = new Edit(Substitute.For<IEnforcementOrderRepository>(),
            Substitute.For<ILegalAuthorityRepository>(), Substitute.For<IEpdContactRepository>());

        var result = await page.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.GetAdminViewAsync(Arg.Any<int>()).Returns(null as EnforcementOrderAdminView);
        var page = new Edit(orderRepo, Substitute.For<ILegalAuthorityRepository>(),
            Substitute.For<IEpdContactRepository>());

        var result = await page.OnGetAsync(-1);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_GivenDeletedItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetEnforcementOrderAdminViewList().First(e => e.Deleted);
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.GetAdminViewAsync(Arg.Any<int>()).Returns(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(orderRepo, Substitute.For<ILegalAuthorityRepository>(),
                Substitute.For<IEpdContactRepository>())
            { TempData = tempData };

        var result = await page.OnGetAsync(item.Id);

        var expected = new DisplayMessage(Context.Warning,
            "This Enforcement Order is deleted and cannot be edited.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(item.Id);
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidId_ReturnsNotFound()
    {
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.GetAdminViewAsync(Arg.Any<int>()).Returns(null as EnforcementOrderAdminView);
        var validator = Substitute.For<IValidator<EnforcementOrderUpdate>>();
        validator.ValidateAsync(Arg.Any<EnforcementOrderUpdate>(), CancellationToken.None)
            .Returns(new ValidationResult());
        var page = new Edit(orderRepo, Substitute.For<ILegalAuthorityRepository>(),
            Substitute.For<IEpdContactRepository>())
        {
            Item = new EnforcementOrderUpdate(),
        };

        var result = await page.OnPostAsync(validator);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenDeletedItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetEnforcementOrderAdminViewList().First(e => e.Deleted);
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.GetAdminViewAsync(Arg.Any<int>()).Returns(item);
        var validator = Substitute.For<IValidator<EnforcementOrderUpdate>>();
        validator.ValidateAsync(Arg.Any<EnforcementOrderUpdate>(), CancellationToken.None)
            .Returns(new ValidationResult());

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(orderRepo, Substitute.For<ILegalAuthorityRepository>(),
            Substitute.For<IEpdContactRepository>())
        {
            TempData = tempData,
            Item = new EnforcementOrderUpdate { Id = item.Id },
        };

        var result = await page.OnPostAsync(validator);

        var expected = new DisplayMessage(Context.Warning,
            "This Enforcement Order is deleted and cannot be edited.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(item.Id);
        }
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var originalItem = ResourceHelper.GetEnforcementOrderAdminViewList().First(e => !e.Deleted);
        var item = new EnforcementOrderUpdate(originalItem);
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.GetAdminViewAsync(Arg.Any<int>()).Returns(originalItem);
        orderRepo.OrderNumberExistsAsync(Arg.Any<string>(), Arg.Any<int?>()).Returns(false);
        var validator = Substitute.For<IValidator<EnforcementOrderUpdate>>();
        validator.ValidateAsync(Arg.Any<EnforcementOrderUpdate>(), CancellationToken.None)
            .Returns(new ValidationResult());

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(orderRepo, Substitute.For<ILegalAuthorityRepository>(),
                Substitute.For<IEpdContactRepository>())
            { TempData = tempData, Item = item };

        var result = await page.OnPostAsync(validator);

        var expected = new DisplayMessage(Context.Success,
            "The Enforcement Order has been successfully updated.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["Id"].Should().Be(originalItem.Id);
        }
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var item = ResourceHelper.GetEnforcementOrderAdminView(1);
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.GetAdminViewAsync(Arg.Any<int>()).Returns(item);
        orderRepo.OrderNumberExistsAsync(Arg.Any<string>(), Arg.Any<int?>()).Returns(false);
        var legalRepo = Substitute.For<ILegalAuthorityRepository>();
        legalRepo.ListAsync().Returns(new List<LegalAuthorityView>());
        var contactRepo = Substitute.For<IEpdContactRepository>();
        contactRepo.ListAsync().Returns(new List<EpdContactView>());

        var validator = Substitute.For<IValidator<EnforcementOrderUpdate>>();
        validator.ValidateAsync(Arg.Any<EnforcementOrderUpdate>(), CancellationToken.None)
            .Returns(new ValidationResult());

        var page = new Edit(orderRepo, legalRepo, contactRepo)
        {
            Item = new EnforcementOrderUpdate(item),
            OriginalOrderNumber = "original order number",
        };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync(validator);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.ModelState.ErrorCount.Should().Be(1);
            page.OriginalOrderNumber.Should().Be("original order number");
        }
    }
}
