using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using FluentValidation.TestHelper;

namespace DomainTests.ValidationTests;

[TestFixture]
public class LegalAuthorityCreateValidatorTests
{
    [Test]
    public async Task SucceedsGivenValidUpdates()
    {
        var command = new LegalAuthorityCreateDto { AuthorityName = "auth" };

        var validator = new LegalAuthorityCreateValidator(Substitute.For<ILegalAuthorityRepository>());

        var result = await validator.TestValidateAsync(command);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }

    [Test]
    public async Task FailsIfMissingRequiredProperties()
    {
        var command = new LegalAuthorityCreateDto { AuthorityName = null };

        var validator = new LegalAuthorityCreateValidator(Substitute.For<ILegalAuthorityRepository>());

        var result = await validator.TestValidateAsync(command);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty().And.HaveCount(1);
            result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
        }
    }

    [Test]
    public async Task FailsWithDuplicateName()
    {
        var command = new LegalAuthorityCreateDto { AuthorityName = "auth" };


        var repoMock = Substitute.For<ILegalAuthorityRepository>();
        repoMock.NameExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);

        var validator = new LegalAuthorityCreateValidator(repoMock);

        var result = await validator.TestValidateAsync(command);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
        }
    }
}
