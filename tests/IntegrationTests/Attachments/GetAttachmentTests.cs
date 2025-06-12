using Enfo.Domain.EnforcementOrders.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.Infrastructure.Attachments;

[TestFixture]
public class GetAttachmentTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var item = new AttachmentView(AttachmentData.Attachments.First(a => !a.Deleted));

        var result = await repository.GetAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.GetAttachmentAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
