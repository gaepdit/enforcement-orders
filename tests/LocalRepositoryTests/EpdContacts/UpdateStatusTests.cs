using Enfo.LocalRepository.Repositories;

namespace LocalRepositoryTests.EpdContacts;

[TestFixture]
public class UpdateStatusTests
{
    [Test]
    public async Task IfStatusChanged_Succeeds([Values] bool newActiveStatus)
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active != newActiveStatus).Id;
        using var repository = new LocalEpdContactRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    public async Task IfStatusUnchanged_Succeeds([Values] bool newActiveStatus)
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active == newActiveStatus).Id;
        using var repository = new LocalEpdContactRepository();

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
            using var repository = new LocalEpdContactRepository();
            await repository.UpdateStatusAsync(id, true);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({id}) not found. (Parameter 'id')")
            .And.ParamName.Should().Be(nameof(id));
    }
}
