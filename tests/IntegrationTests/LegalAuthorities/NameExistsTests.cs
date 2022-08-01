using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using static EnfoTests.Infrastructure.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure.LegalAuthorities;

[TestFixture]
public class NameExistsTests
{
    [Test]
    public async Task NameExists_WhenNameExists_ReturnsTrue()
    {
        var item = LegalAuthorityData.LegalAuthorities.First();
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(item.AuthorityName)).Should().BeTrue();
    }

    [Test]
    public async Task NameExists_WhenNameNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(Guid.NewGuid().ToString())).Should().BeFalse();
    }
}
