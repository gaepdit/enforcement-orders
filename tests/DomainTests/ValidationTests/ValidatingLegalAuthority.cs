using Enfo.Domain.Resources.LegalAuthority;
using Enfo.Domain.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

namespace EnfoTests.Domain.ValidationTests;

public class ValidatingLegalAuthority
{
    // Sample data for create
    private readonly LegalAuthorityCommand _command = new()
    {
        Id = 1,
        AuthorityName = "auth",
    };

    [Fact]
    public async Task SucceedsGivenValidUpdates()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var validator = new LegalAuthorityValidator(repository);

        var result = await validator.TestValidateAsync(_command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task FailsIfMissingRequiredProperties()
    {
        _command.AuthorityName = null;

        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var validator = new LegalAuthorityValidator(repository);

        var result = await validator.TestValidateAsync(_command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
        result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
    }

    [Fact]
    public async Task FailsWithDuplicateName()
    {
        _command.AuthorityName = GetLegalAuthorities.Last().AuthorityName;

        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var validator = new LegalAuthorityValidator(repository);

        var result = await validator.TestValidateAsync(_command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
    }
}
