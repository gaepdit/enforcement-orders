using EnfoTests.EfRepository.Helpers;

namespace EnfoTests.EfRepository.EnforcementOrderTests;

public class OrderNumberExistsTests
{
    [Test]
    public async Task OrderNumberExists_GivenExists_ReturnsTrue()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(EnforcementOrderData.EnforcementOrders[0].OrderNumber))
            .Should().BeTrue();
    }

    [Test]
    public async Task OrderNumberExists_GivenNotExists_ReturnsFalse()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(Guid.NewGuid().ToString()))
            .Should().BeFalse();
    }

    [Test]
    public async Task OrderNumberExists_GivenExistsAndIgnore_ReturnsFalse()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(EnforcementOrderData.EnforcementOrders[0].OrderNumber,
                EnforcementOrderData.EnforcementOrders[0].Id))
            .Should().BeFalse();
    }
}
