using Enfo.LocalRepository.Repositories;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.LocalRepositoryTests.EpdContacts;

[TestFixture]
public class ListTests
{
    [Test]
    public async Task ByDefault_ReturnsOnlyActive()
    {
        using var repository = new LocalEpdContactRepository();
        var result = await repository.ListAsync();
        result.Should().BeEquivalentTo(EpdContactData.EpdContacts.Where(e => e.Active));
    }

    [Test]
    public async Task IfIncludeAll_ReturnsAll()
    {
        using var repository = new LocalEpdContactRepository();
        var result = await repository.ListAsync(true);
        result.Should().BeEquivalentTo(EpdContactData.EpdContacts);
    }
}
