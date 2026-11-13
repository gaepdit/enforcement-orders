using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

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
        var page = new EditModel(repo) { Id = item.Id };

        await page.OnGetAsync();

        using (new AssertionScope())
        {
            page.Item.Should().BeEquivalentTo(new LegalAuthorityUpdateDto()
                { Active = item.Active, AuthorityName = item.AuthorityName });
            page.Id.Should().Be(item.Id);
            page.OriginalName.Should().Be(item.AuthorityName);
        }
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var repo = Substitute.For<ILegalAuthorityRepository>();
        var page = new EditModel(repo);

        var result = await page.OnGetAsync();

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
        repo.GetAsync(Arg.Any<int>()).Returns(null as LegalAuthorityViewDto);
        var page = new EditModel(repo) { Id = -1 };

        var result = await page.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            page.Item.Should().BeNull();
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidId_ReturnsNotFound()
    {
        var repo = Substitute.For<ILegalAuthorityRepository>();
        repo.GetAsync(Arg.Any<int>()).Returns(null as LegalAuthorityViewDto);
        var validator = Substitute.For<IValidator<LegalAuthorityUpdateDto>>();
        validator.ValidateAsync(Arg.Any<LegalAuthorityUpdateDto>(), CancellationToken.None)
            .Returns(new ValidationResult());
        var page = new EditModel(repo) { Id = 0 };

        var result = await page.OnPostAsync(validator);

        result.Should().BeOfType<BadRequestResult>();
    }
}
