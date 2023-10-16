using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class UpdateStatusTests
{
    [Test]
    public async Task IfStatusChanged_Succeeds([Values] bool newActiveStatus)
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active != newActiveStatus).Id;
        using var repository = new LocalLegalAuthorityRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    public async Task IfStatusUnchanged_Succeeds([Values] bool newActiveStatus)
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active == newActiveStatus).Id;
        using var repository = new LocalLegalAuthorityRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    public async Task FromMissingId_ThrowsException()
    {
        const int id = -1;

        var action = async () =>
        {
            using var repository = new LocalLegalAuthorityRepository();
            await repository.UpdateStatusAsync(id, true);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({id}) not found. (Parameter 'id')")
            .And.ParamName.Should().Be(nameof(id));
    }
}
