using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests.Pages.Admin.Maintenance.LegalAuthorities;

[TestFixture]
public class EditTests
{
    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var item = ResourceHelper.GetLegalAuthorityViewList()[0];
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(item.Id).Returns(item);
        var page = new Edit(repo);

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
        var repo = Substitute.For<ILegalAuthorityRepository>();
        var page = new Edit(repo);

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
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(null as LegalAuthorityView);
        var page = new Edit(repo);

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
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(item);

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(repo) { TempData = tempData };

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
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(null as LegalAuthorityView);
        var validator = Substitute.For<IValidator<LegalAuthorityCommand>>();
        validator.ValidateAsync(Arg.Any<LegalAuthorityCommand>(), CancellationToken.None)
            .Returns(new ValidationResult());
        var page = new Edit(repo) { Item = new LegalAuthorityCommand { Id = 0 } };

        var result = await page.OnPostAsync(validator);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenInactiveItem_RedirectsWithDisplayMessage()
    {
        var item = ResourceHelper.GetLegalAuthorityViewList().Single(e => !e.Active);
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(item);
        var validator = Substitute.For<IValidator<LegalAuthorityCommand>>();
        validator.ValidateAsync(Arg.Any<LegalAuthorityCommand>(), CancellationToken.None)
            .Returns(new ValidationResult());

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(repo)
        {
            TempData = tempData,
            Item = new LegalAuthorityCommand(item),
        };

        var result = await page.OnPostAsync(validator);

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
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(ResourceHelper.GetLegalAuthorityViewList()[0]);
        repo.NameExistsAsync(Arg.Any<string>()).Returns(false);
        var validator = Substitute.For<IValidator<LegalAuthorityCommand>>();
        validator.ValidateAsync(Arg.Any<LegalAuthorityCommand>(), CancellationToken.None)
            .Returns(new ValidationResult());

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Edit(repo) { TempData = tempData, Item = item };

        var result = await page.OnPostAsync(validator);

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
