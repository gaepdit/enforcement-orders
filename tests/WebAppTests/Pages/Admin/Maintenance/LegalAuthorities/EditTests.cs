using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;
using Enfo.WebApp.Platform.RazorHelpers;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[TestFixture]
public class EditTests
{
    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var item = ResourceHelper.GetLegalAuthorityViewList()[0];
        var repo = new Mock<ILegalAuthorityRepository>();
        repo.Setup(l => l.GetAsync(item.Id)).ReturnsAsync(item);
        var page = new Edit(repo.Object);

        await page.OnGetAsync(item.Id);

        using (new AssertionScope())
        {
            page.Item.Should().BeEquivalentTo(new LegalAuthorityCommand(item));
            page.Item.Id.Should().Be(item.Id);
            page.OriginalName.Should().Be(item.AuthorityName);
        }
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var repo = new Mock<ILegalAuthorityRepository>();
        var page = new Edit(repo.Object);

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
        var repo = new Mock<ILegalAuthorityRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as LegalAuthorityView);
        var page = new Edit(repo.Object);

        var result = await page.OnGetAsync(-1);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_GivenInactiveItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetLegalAuthorityViewList().Single(e => !e.Active);
        var repo = new Mock<ILegalAuthorityRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Edit(repo.Object) { TempData = tempData };

        var result = await page.OnGetAsync(item.Id);

        var expected = new DisplayMessage(Context.Warning,
            $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidId_ReturnsNotFound()
    {
        var repo = new Mock<ILegalAuthorityRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>())).ReturnsAsync(null as LegalAuthorityView);
        var validator = new Mock<IValidator<LegalAuthorityCommand>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<LegalAuthorityCommand>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new Edit(repo.Object) { Item = new LegalAuthorityCommand { Id = 0 } };

        var result = await page.OnPostAsync(validator.Object);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenInactiveItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetLegalAuthorityViewList().Single(e => !e.Active);
        var repo = new Mock<ILegalAuthorityRepository>();
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(item);
        var validator = new Mock<IValidator<LegalAuthorityCommand>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<LegalAuthorityCommand>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Edit(repo.Object)
        {
            TempData = tempData,
            Item = new LegalAuthorityCommand(item),
        };

        var result = await page.OnPostAsync(validator.Object);

        var expected = new DisplayMessage(Context.Warning,
            $"Inactive {Edit.ThisOption.PluralName} cannot be edited.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var item = new LegalAuthorityCommand(ResourceHelper.GetLegalAuthorityViewList()[0]);
        var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
        repo.Setup(l => l.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(ResourceHelper.GetLegalAuthorityViewList()[0]);
        repo.Setup(l => l.NameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        var validator = new Mock<IValidator<LegalAuthorityCommand>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<LegalAuthorityCommand>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var page = new Edit(repo.Object) { TempData = tempData, Item = item };

        var result = await page.OnPostAsync(validator.Object);

        var expected = new DisplayMessage(Context.Success,
            $"{item.AuthorityName} successfully updated.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }
}
