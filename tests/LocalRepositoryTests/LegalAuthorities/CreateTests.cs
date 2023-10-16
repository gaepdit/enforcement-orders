using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class CreateTests
{
    [Test]
    public async Task FromValidItem_AddsNew()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = "New Item" };
        var expectedId = LegalAuthorityData.LegalAuthorities.Max(e => e.Id) + 1;
        var repository = new LocalLegalAuthorityRepository();

        var itemId = await repository.CreateAsync(resource);

        var item = new LegalAuthority(resource) { Id = itemId };
        var expected = new LegalAuthorityView(item);
        var newItem = await repository.GetAsync(itemId);

        using (new AssertionScope())
        {
            itemId.Should().Be(expectedId);
            newItem.Should().BeEquivalentTo(expected);
        }
    }

    [Test]
    public async Task FromInvalidItem_ThrowsException()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = null };

        var action = async () =>
        {
            using var repository = new LocalLegalAuthorityRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(LegalAuthorityCommand.AuthorityName));
    }
}
