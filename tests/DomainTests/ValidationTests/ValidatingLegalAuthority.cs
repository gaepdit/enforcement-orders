using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.LegalAuthorities.Resources.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EnfoTests.Domain.ValidationTests;

[TestFixture]
public class ValidatingLegalAuthority
{
    [Test]
    public async Task SucceedsGivenValidUpdates()
    {
        var command = new LegalAuthorityCommand { Id = 1, AuthorityName = "auth" };

        var validator = new LegalAuthorityValidator(new Mock<ILegalAuthorityRepository>().Object);

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
        var command = new LegalAuthorityCommand { Id = 1, AuthorityName = null };

        var validator = new LegalAuthorityValidator(new Mock<ILegalAuthorityRepository>().Object);

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
        var command = new LegalAuthorityCommand { Id = 1, AuthorityName = "auth" };

        var repoMock = new Mock<ILegalAuthorityRepository>();
        repoMock.Setup(l => l.NameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var validator = new LegalAuthorityValidator(repoMock.Object);

        var result = await validator.TestValidateAsync(command);

        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(e => e.AuthorityName);
        });
    }
}
