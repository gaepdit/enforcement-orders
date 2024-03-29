﻿using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.EpdContacts;

[TestFixture]
public class ExistsTests
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        using var repository = new LocalEpdContactRepository();
        var result = await repository.ExistsAsync(EpdContactData.EpdContacts[0].Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotExists_ReturnsFalse()
    {
        using var repository = new LocalEpdContactRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }
}
