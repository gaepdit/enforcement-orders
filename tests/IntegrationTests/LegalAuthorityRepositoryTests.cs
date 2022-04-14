using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Resources;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure;

public class LegalAuthorityRepositoryTests
{
    // GetAsync

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Get_WhenItemExists_ReturnsItem(int id)
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var item = new LegalAuthorityView(GetLegalAuthorities.Single(e => e.Id == id));

        var result = await repository.GetAsync(id);

        result.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var item = await repository.GetAsync(-1);
        item.Should().BeNull();
    }

    // ListAsync

    [Fact]
    public async Task List_ByDefault_ReturnsOnlyActive()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var items = await repository.ListAsync();
        items.Should().HaveCount(GetLegalAuthorities.Count(e => e.Active));
        items[0].Should().BeEquivalentTo(new LegalAuthorityView(GetLegalAuthorities.First(e => e.Active)));
    }

    [Fact]
    public async Task List_IfIncludeAll_ReturnsAll()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var items = await repository.ListAsync(true);
        items.Should().HaveCount(GetLegalAuthorities.Count());
        items.Should().BeEquivalentTo(GetLegalAuthorities.Select(e => new LegalAuthorityView(e)));
    }

    // CreateAsync

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
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

    [Fact]
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

    [Fact]
    public async Task Exists_WhenItemExists_ReturnsTrue()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var result = await repository.ExistsAsync(GetLegalAuthorities.First().Id);
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task Exists_WhenItemNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var result = await repository.ExistsAsync(-1);
        result.ShouldBeFalse();
    }

    // NameExistsAsync

    [Fact]
    public async Task NameExists_WhenNameExists_ReturnsTrue()
    {
        var item = GetLegalAuthorities.First();
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(item.AuthorityName)).ShouldBeTrue();
    }

    [Fact]
    public async Task NameExists_WhenNameNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(Guid.NewGuid().ToString())).ShouldBeFalse();
    }

    [Fact]
    public async Task NameExists_WhenNameExists_WithMatchingId_ReturnsFalse()
    {
        var item = GetLegalAuthorities.First();
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(item.AuthorityName, item.Id)).ShouldBeFalse();
    }

    [Fact]
    public async Task NameExists_WhenNameExists_WithNonMatchingId_ReturnsTrue()
    {
        var item = GetLegalAuthorities.First();
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(item.AuthorityName, -1)).ShouldBeTrue();
    }
}
