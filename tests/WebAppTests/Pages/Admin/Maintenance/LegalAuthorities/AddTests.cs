using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;
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
using System.Threading;
using System.Threading.Tasks;

namespace EnfoTests.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[TestFixture]
public class AddTests
{
    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var item = new LegalAuthorityCommand { AuthorityName = "test" };
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.NameExistsAsync(Arg.Any<string>()).Returns(false);
        repo.CreateAsync(Arg.Any<LegalAuthorityCommand>()).Returns(1);
        var validator = Substitute.For<IValidator<LegalAuthorityCommand>>();
        validator.ValidateAsync(Arg.Any<LegalAuthorityCommand>(), CancellationToken.None)
            .Returns(new ValidationResult());

        // Initialize Page TempData
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        var page = new Add { TempData = tempData, Item = item };

        var result = await page.OnPostAsync(repo, validator);

        var expected = new DisplayMessage(Context.Success,
            $"{item.AuthorityName} successfully added.");

        using (new AssertionScope())
        {
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expected);
            page.HighlightId.Should().Be(1);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenModelError_ReturnsPageWithModelError()
    {
        var repo = Substitute.For<ILegalAuthorityRepository>();
        var validator = Substitute.For<IValidator<LegalAuthorityCommand>>();
        validator.ValidateAsync(Arg.Any<LegalAuthorityCommand>(), CancellationToken.None)
            .Returns(new ValidationResult());
        var page = new Add { Item = new LegalAuthorityCommand() };
        page.ModelState.AddModelError("key", "message");

        var result = await page.OnPostAsync(repo, validator);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
