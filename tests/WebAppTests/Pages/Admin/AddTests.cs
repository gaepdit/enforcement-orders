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
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin;

[TestFixture]
public class AddTests
{
    private static EnforcementOrderCreate GetValidEnforcementOrderCreate() => new()
    {
        Cause = "xyz-" + Guid.NewGuid(),
        CommentContactId = 2000,
        CommentPeriodClosesDate = new DateTime(2012, 11, 15),
        County = "Liberty",
        ExecutedDate = new DateTime(1998, 06, 29),
        ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
        FacilityName = "xyz-" + Guid.NewGuid(),
        HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
        HearingContactId = 2000,
        HearingDate = new DateTime(2012, 11, 15),
        HearingLocation = "xyz-" + Guid.NewGuid(),
        IsHearingScheduled = true,
        LegalAuthorityId = 1,
        OrderNumber = "EPD-ACQ-7936",
        ProposedOrderPostedDate = new DateTime(2012, 10, 16),
        Progress = PublicationProgress.Published,
        Requirements = "xyz-" + Guid.NewGuid(),
        SettlementAmount = 2000,
    };

    [Test]
    public async Task OnGet_ReturnsWithDefaultCreateResource()
    {
        var legalRepo = Substitute.For<ILegalAuthorityRepository>();
        legalRepo.ListAsync(false).Returns(new List<LegalAuthorityView>());
        var contactRepo = Substitute.For<IEpdContactRepository>();
        contactRepo.ListAsync(false).Returns(new List<EpdContactView>());
        var page = new Add(Substitute.For<IEnforcementOrderRepository>(), legalRepo, contactRepo);

        await page.OnGetAsync();
        page.Item.Should().BeEquivalentTo(new EnforcementOrderCreate());
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var item = GetValidEnforcementOrderCreate();
        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        // Mock repos
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.OrderNumberExistsAsync(Arg.Any<string>(), Arg.Any<int?>()).Returns(false);
        orderRepo.CreateAsync(item).Returns(9);
        var validator = Substitute.For<IValidator<EnforcementOrderCreate>>();
        validator.ValidateAsync(Arg.Any<EnforcementOrderCreate>(), CancellationToken.None).Returns(new ValidationResult());
        // Construct Page
        var page = new Add(orderRepo, Substitute.For<ILegalAuthorityRepository>(), Substitute.For<IEpdContactRepository>())
            { TempData = tempData, Item = item };

        var result = await page.OnPostAsync(validator);

        var expected = new DisplayMessage(Context.Success,
            "The new Enforcement Order has been successfully added.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(9);
        }
    }


    [Test]
    public async Task OnPost_WithAttachment_ReturnsRedirectWithDisplayMessage()
    {
        var item = new EnforcementOrderCreate
        {
            Cause = "xyz-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "xyz-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2000,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "xyz-" + Guid.NewGuid(),
            IsHearingScheduled = true,
            LegalAuthorityId = 1,
            OrderNumber = "EPD-ACQ-7936",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            Progress = PublicationProgress.Published,
            Requirements = "xyz-" + Guid.NewGuid(),
            SettlementAmount = 2000,
            Attachment = new FormFile(Stream.Null, 0, 3, "test3", "test3.pdf"),
        };

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        // Mock repos
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.OrderNumberExistsAsync(Arg.Any<string>(), Arg.Any<int?>()).Returns(false);
        orderRepo.CreateAsync(item).Returns(9);
        var validator = Substitute.For<IValidator<EnforcementOrderCreate>>();
        validator.ValidateAsync(Arg.Any<EnforcementOrderCreate>(), CancellationToken.None).Returns(new ValidationResult());
        // Construct Page
        var page = new Add(orderRepo, Substitute.For<ILegalAuthorityRepository>(), Substitute.For<IEpdContactRepository>())
            { TempData = tempData, Item = item };

        var result = await page.OnPostAsync(validator);

        var expected = new DisplayMessage(Context.Success,
            "The new Enforcement Order has been successfully added.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(9);
        }
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var item = GetValidEnforcementOrderCreate();
        // Mock repos
        var legalRepo = Substitute.For<ILegalAuthorityRepository>();
        legalRepo.ListAsync(false).Returns(new List<LegalAuthorityView>());
        var contactRepo = Substitute.For<IEpdContactRepository>();
        contactRepo.ListAsync(false).Returns(new List<EpdContactView>());
        var orderRepo = Substitute.For<IEnforcementOrderRepository>();
        orderRepo.OrderNumberExistsAsync(Arg.Any<string>(), Arg.Any<int?>()).Returns(false);
        var validator = Substitute.For<IValidator<EnforcementOrderCreate>>();
        validator.ValidateAsync(Arg.Any<EnforcementOrderCreate>(), CancellationToken.None).Returns(new ValidationResult());
        // Construct Page
        var page = new Add(orderRepo, legalRepo, contactRepo)
            { Item = item };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync(validator);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.ModelState.ErrorCount.Should().Be(1);
        }
    }
}
