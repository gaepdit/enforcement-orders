using Enfo.LocalRepository.EpdContacts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EpdContacts;

[TestFixture]
public class UpdateStatusTests
{
    [Test]
    public async Task IfStatusChanged_Succeeds([Values] bool newActiveStatus)
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active != newActiveStatus).Id;
        using var repository = new EpdContactRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    public async Task IfStatusUnchanged_Succeeds([Values] bool newActiveStatus)
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active == newActiveStatus).Id;
        using var repository = new EpdContactRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    public async Task FromMissingId_ThrowsException()
    {
        const int itemId = -1;

        var action = async () =>
        {
            using var repository = new EpdContactRepository();
            await repository.UpdateStatusAsync(itemId, true);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'id')")
            .And.ParamName.Should().Be("id");
    }
}
