﻿using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.LocalRepository.LegalAuthorities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class CreateTests
{
    [Test]
    public async Task FromValidItem_AddsNew()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = "New Item" };
        var expectedId = LegalAuthorityData.LegalAuthorities.Max(e => e.Id) + 1;
        var repository = new LegalAuthorityRepository();

        var result = await repository.CreateAsync(resource);
        var newItem = await repository.GetAsync(result);

        Assert.Multiple(() =>
        {
            result.Should().Be(expectedId);
            newItem.Active.Should().BeTrue();
            newItem.AuthorityName.Should().Be(resource.AuthorityName);
        });
    }

    [Test]
    public async Task FromInvalidItem_ThrowsException()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = null };

        var action = async () =>
        {
            using var repository = new LegalAuthorityRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(LegalAuthorityCommand.AuthorityName));
    }
}
