using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using static EnfoTests.Infrastructure.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure.EpdContacts;

[TestFixture]
public class ExistsTests
{
    [Test]
    public async Task Exists_GivenExists_ReturnsTrue()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var result = await repository.ExistsAsync(EpdContactData.EpdContacts.First().Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_GivenNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }
}
