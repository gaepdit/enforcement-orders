using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.LegalAuthorities.Resources.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

namespace EnfoTests.Domain.ValidationTests;

[TestFixture]
public class ValidatingLegalAuthority
{
    [Test]
    public async Task SucceedsGivenValidUpdates()
    {
    var command = new LegalAuthorityCommand
     {
         Id = 1,
         AuthorityName = "auth",
     };

     using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var validator = new LegalAuthorityValidator(repository);

        var result = await validator.TestValidateAsync(command);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        });
    }

    [Test]
    public async Task FailsIfMissingRequiredProperties()
    {
        var command = new LegalAuthorityCommand
        {
            Id = 1,
            AuthorityName = null,
        };

        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var validator = new LegalAuthorityValidator(repository);

        var result = await validator.TestValidateAsync(command);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty().And.HaveCount(1);
            result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
        });
    }

    [Test]
    public async Task FailsWithDuplicateName()
    {
        var command = new LegalAuthorityCommand
        {
            Id = 1,
            AuthorityName =  GetLegalAuthorities.Last().AuthorityName,
        };

        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var validator = new LegalAuthorityValidator(repository);

        var result = await validator.TestValidateAsync(command);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
        });
    }
}
