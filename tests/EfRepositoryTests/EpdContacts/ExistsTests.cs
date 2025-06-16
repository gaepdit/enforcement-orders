using EnfoTests.EfRepository.Helpers;

namespace EnfoTests.EfRepository.EpdContacts;

[TestFixture]
public class ExistsTests
{
    [Test]
    public async Task Exists_GivenExists_ReturnsTrue()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEpdContactRepository();
        var result = await repository.ExistsAsync(EpdContactData.EpdContacts[0].Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_GivenNotExists_ReturnsFalse()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEpdContactRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }
}
