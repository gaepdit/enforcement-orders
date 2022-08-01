using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.Infrastructure.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.LegalAuthorities;

public class CreateTests
{
    [Test]
    public async Task Create_FromValidItem_AddsNew()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = "New Item" };

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        var itemId = await repository.CreateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = new LegalAuthority(resource) { Id = itemId };
        var expected = new LegalAuthorityView(item);
        (await repository.GetAsync(itemId))
            .Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Create_FromInvalidItem_ThrowsException()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = null };

        var action = async () =>
        {
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetLegalAuthorityRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(LegalAuthorityCommand.AuthorityName));
    }
}
