﻿using Enfo.LocalRepository.EpdContacts;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EpdContacts;

[TestFixture]
public class ListTests
{
    [Test]
    public async Task ByDefault_ReturnsOnlyActive()
    {
        using var repository = new EpdContactRepository();
        var result = await repository.ListAsync();
        result.Should().BeEquivalentTo(EpdContactData.EpdContacts.Where(e => e.Active));
    }

    [Test]
    public async Task IfIncludeAll_ReturnsAll()
    {
        using var repository = new EpdContactRepository();
        var result = await repository.ListAsync(true);
        result.Should().BeEquivalentTo(EpdContactData.EpdContacts);
    }
}
