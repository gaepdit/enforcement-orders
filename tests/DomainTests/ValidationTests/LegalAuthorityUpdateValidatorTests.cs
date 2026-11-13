using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using FluentValidation;
using FluentValidation.TestHelper;

namespace DomainTests.ValidationTests;

[TestFixture]
public class LegalAuthorityUpdateValidatorTests
{
    private static ValidationContext<LegalAuthorityUpdateDto> GetContext(LegalAuthorityUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = 1 } };

    [Test]
    public async Task SucceedsGivenValidUpdates()
    {
        var model = new LegalAuthorityUpdateDto { AuthorityName = "auth" };
        var validator = new LegalAuthorityUpdateValidator(Substitute.For<ILegalAuthorityRepository>());

        var result = await validator.TestValidateAsync(GetContext(model));

        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }

    [Test]
    public async Task FailsIfMissingRequiredProperties()
    {
        var model = new LegalAuthorityUpdateDto { AuthorityName = null };
        var validator = new LegalAuthorityUpdateValidator(Substitute.For<ILegalAuthorityRepository>());

        var result = await validator.TestValidateAsync(GetContext(model));

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty().And.HaveCount(1);
            result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
        }
    }

    [Test]
    public async Task SucceedsIfDuplicateNameIsSameItem()
    {
        var model = new LegalAuthorityUpdateDto { AuthorityName = "auth" };

        var repoMock = Substitute.For<ILegalAuthorityRepository>();
        repoMock.FindIdByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(1);

        var validator = new LegalAuthorityUpdateValidator(repoMock);

        var result = await validator.TestValidateAsync(GetContext(model));

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task FailsWithDuplicateName()
    {
        var model = new LegalAuthorityUpdateDto { AuthorityName = "auth" };

        var repoMock = Substitute.For<ILegalAuthorityRepository>();
        repoMock.FindIdByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(2);

        var validator = new LegalAuthorityUpdateValidator(repoMock);

        var result = await validator.TestValidateAsync(GetContext(model));

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
        }
    }
}
