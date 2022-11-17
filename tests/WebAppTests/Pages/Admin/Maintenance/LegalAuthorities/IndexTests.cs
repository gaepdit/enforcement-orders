﻿using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;
using Enfo.WebApp.Platform.RazorHelpers;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[TestFixture]
public class IndexTests
{
    [Test]
    public async Task OnGet_ReturnsWithOrder()
    {
        var list = ResourceHelper.GetLegalAuthorityViewList();
        var repo = new Mock<ILegalAuthorityRepository>();
        repo.Setup(l => l.ListAsync(true)).ReturnsAsync(list);
        var page = new Index(repo.Object);

        await page.OnGetAsync();

        using (new AssertionScope())
        {
            page.Items.Should().BeEquivalentTo(list);
            page.Message.Should().BeNull();
        }
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Index(repo.Object) { TempData = tempData };

        page.TempData.SetDisplayMessage(Context.Info, "Info message");
        await page.OnGetAsync();

        var expected = new DisplayMessage(Context.Info, "Info message");
        page.Message.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task OnPost_ReturnsRedirectWithDisplayMessage()
    {
        var item = ResourceHelper.GetLegalAuthorityViewList()[0];
        var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Index(repo.Object) { TempData = tempData };

        var result = await page.OnPostAsync(item.Id);

        var expected = new DisplayMessage(Context.Success,
            $"{item.AuthorityName} successfully {(item.Active ? "deactivated" : "restored")}.");

        using (new AssertionScope())
        {
            page.TempData?.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenNullId_ReturnsBadRequest()
    {
        var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
        var page = new Index(repo.Object);

        var result = await page.OnPostAsync(null);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidId_ReturnsNotFound()
    {
        var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(null as LegalAuthorityView);
        var page = new Index(repo.Object);

        var result = await page.OnPostAsync(1);

        result.Should().BeOfType<NotFoundResult>();
    }
}
