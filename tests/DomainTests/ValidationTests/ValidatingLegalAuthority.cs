using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.LegalAuthorities.Resources.Validation;
using FluentValidation.TestHelper;

namespace EnfoTests.Domain.ValidationTests;

[TestFixture]
public class ValidatingLegalAuthority
{
    [Test]
    public async Task SucceedsGivenValidUpdates()
    {
        var command = new LegalAuthorityCommand { Id = 1, AuthorityName = "auth" };

        var validator = new LegalAuthorityValidator(Substitute.For<ILegalAuthorityRepository>());

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
        var command = new LegalAuthorityCommand { Id = 1, AuthorityName = null };

        var validator = new LegalAuthorityValidator(Substitute.For<ILegalAuthorityRepository>());

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
        var command = new LegalAuthorityCommand { Id = 1, AuthorityName = "auth" };

        var repoMock = Substitute.For<ILegalAuthorityRepository>();
        repoMock.NameExistsAsync(Arg.Any<string>()).Returns(true);

        var validator = new LegalAuthorityValidator(repoMock);

        var result = await validator.TestValidateAsync(command);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
        }
    }
}
