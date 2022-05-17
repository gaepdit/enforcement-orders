using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Resources;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure;

[TestFixture]
public class LegalAuthorityRepositoryTests
{
    // GetAsync

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    public async Task Get_WhenItemExists_ReturnsItem(int id)
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var item = new LegalAuthorityView(GetLegalAuthorities.Single(e => e.Id == id));

        var result = await repository.GetAsync(id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var item = await repository.GetAsync(-1);
        item.Should().BeNull();
    }

    // ListAsync

    [Test]
    public async Task List_ByDefault_ReturnsOnlyActive()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var items = await repository.ListAsync();

        Assert.Multiple(() =>
        {
            items.Should().HaveCount(GetLegalAuthorities.Count(e => e.Active));
            items[0].Should().BeEquivalentTo(new LegalAuthorityView(GetLegalAuthorities.First(e => e.Active)));
        });
    }

    [Test]
    public async Task List_IfIncludeAll_ReturnsAll()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var items = await repository.ListAsync(true);

        Assert.Multiple(() =>
        {
            items.Should().HaveCount(GetLegalAuthorities.Count());
            items.Should().BeEquivalentTo(GetLegalAuthorities.Select(e => new LegalAuthorityView(e)));
        });
    }

    // CreateAsync

    [Test]
    public async Task Create_FromValidItem_AddsNew()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = "New Item" };

        using var repositoryHelper = CreateRepositoryHelper();
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
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(LegalAuthorityCommand.AuthorityName));
    }

    // UpdateAsync

    [Test]
    public async Task Update_FromValidItem_Updates()
    {
        var itemId = GetLegalAuthorities.First(e => e.Active).Id;
        var resource = new LegalAuthorityCommand { Id = itemId, AuthorityName = "New Name" };

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        await repository.UpdateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Should().BeEquivalentTo(resource);
    }

    [Test]
    public async Task Update_WithNoChanges_Succeeds()
    {
        var original = GetLegalAuthorities.First(e => e.Active);
        var resource = new LegalAuthorityCommand { Id = original.Id, AuthorityName = original.AuthorityName };

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        await repository.UpdateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(original.Id);
        item.Should().BeEquivalentTo(new LegalAuthorityView(original));
    }

    [Test]
    public async Task Update_FromInvalidItem_ThrowsException()
    {
        var itemId = GetLegalAuthorities.First(e => e.Active).Id;
        var resource = new LegalAuthorityCommand { Id = itemId, AuthorityName = null };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(LegalAuthorityCommand.AuthorityName));
    }

    [Test]
    public async Task Update_WithMissingId_ThrowsException()
    {
        const int itemId = -1;
        var itemUpdate = new LegalAuthorityCommand { Id = itemId, AuthorityName = "New Name" };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            await repository.UpdateAsync(itemUpdate);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be("resource");
    }

    // UpdateStatusAsync

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task UpdateStatus_IfChangeStatus_Succeeds(bool newActiveStatus)
    {
        var itemId = GetLegalAuthorities.First(e => e.Active != newActiveStatus).Id;

        using var repositoryHelper = CreateRepositoryHelper();
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
        var itemId = GetLegalAuthorities.First(e => e.Active == newActiveStatus).Id;

        using var repositoryHelper = CreateRepositoryHelper();
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
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            await repository.UpdateStatusAsync(itemId, true);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'id')")
            .And.ParamName.Should().Be("id");
    }

    // ExistsAsync

    [Test]
    public async Task Exists_WhenItemExists_ReturnsTrue()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var result = await repository.ExistsAsync(GetLegalAuthorities.First().Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_WhenItemNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }

    // NameExistsAsync

    [Test]
    public async Task NameExists_WhenNameExists_ReturnsTrue()
    {
        var item = GetLegalAuthorities.First();
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(item.AuthorityName)).Should().BeTrue();
    }

    [Test]
    public async Task NameExists_WhenNameNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(Guid.NewGuid().ToString())).Should().BeFalse();
    }

    [Test]
    public async Task NameExists_WhenNameExists_WithMatchingId_ReturnsFalse()
    {
        var item = GetLegalAuthorities.First();
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(item.AuthorityName, item.Id)).Should().BeFalse();
    }

    [Test]
    public async Task NameExists_WhenNameExists_WithNonMatchingId_ReturnsTrue()
    {
        var item = GetLegalAuthorities.First();
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(item.AuthorityName, -1)).Should().BeTrue();
    }
}
