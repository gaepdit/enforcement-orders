using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class UpdateTests
{
    [Test]
    public async Task FromValidItem_Updates()
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active).Id;
        var resource = new LegalAuthorityCommand { Id = itemId, AuthorityName = "New Name" };
        using var repository = new LocalLegalAuthorityRepository();

        await repository.UpdateAsync(resource);

        var updatedItem = await repository.GetAsync(itemId);
        updatedItem.Should().BeEquivalentTo(resource);
    }

    [Test]
    public async Task WithNoChanges_Succeeds()
    {
        var item = LegalAuthorityData.LegalAuthorities.First(e => e.Active);
        var resource = new LegalAuthorityCommand { Id = item.Id, AuthorityName = item.AuthorityName };
        using var repository = new LocalLegalAuthorityRepository();

        await repository.UpdateAsync(resource);

        var updatedItem = await repository.GetAsync(item.Id);
        updatedItem.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task FromInvalidItem_ThrowsException()
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active).Id;
        var resource = new LegalAuthorityCommand { Id = itemId, AuthorityName = null };

        var action = async () =>
        {
            using var repository = new LocalLegalAuthorityRepository();
            await repository.UpdateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WithMissingId_ThrowsException()
    {
        const int itemId = -1;
        var resource = new LegalAuthorityCommand { Id = itemId, AuthorityName = "New Name" };

        var action = async () =>
        {
            using var repository = new LocalLegalAuthorityRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be(nameof(resource));
    }
}
