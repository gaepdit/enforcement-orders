namespace EnfoTests.Infrastructure.LegalAuthorities;

public class UpdateStatusTests
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task UpdateStatus_IfChangeStatus_Succeeds(bool newActiveStatus)
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active != newActiveStatus).Id;

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task UpdateStatus_IfStatusUnchanged_Succeeds(bool newActiveStatus)
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active == newActiveStatus).Id;

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    public async Task UpdateStatus_FromMissingId_ThrowsException()
    {
        const int itemId = -1;

        var action = async () =>
        {
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();
            await repository.UpdateStatusAsync(itemId, true);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'id')")
            .And.ParamName.Should().Be("id");
    }
}
