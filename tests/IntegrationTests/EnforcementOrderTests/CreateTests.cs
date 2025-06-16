using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Microsoft.AspNetCore.Http;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class CreateTests
{
    [Test]
    public async Task CreateOrder_AddsNewItem()
    {
        // Arrange
        EnforcementOrderCreate resource = new()
        {
            Cause = "Cause of order",
            Requirements = "Requirements of order",
            FacilityName = "Facility 4",
            County = "Fulton",
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities[0].Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-4",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts[0].Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        // Act
        var itemId = await repository.CreateAsync(resource);

        // Assert
        repositoryHelper.ClearChangeTracker();

        var order = new EnforcementOrder(resource) { Id = itemId };
        var expected = new EnforcementOrderAdminView(ResourceHelper.FillNavigationProperties(order));

        var item = await repository.GetAdminViewAsync(itemId);
        item.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FromInvalidItem_ThrowsException()
    {
        EnforcementOrderCreate resource = new()
        {
            Cause = "Cause of order",
            Requirements = "Requirements of order",
            FacilityName = "Facility 2",
            County = null,
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities[0].Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-2",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts[0].Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        var action = async () =>
        {
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetEnforcementOrderRepository();
            await repository.CreateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task ValidItemWithAttachments_AddsAll()
    {
        // Arrange
        EnforcementOrderCreate resource = new()
        {
            Cause = "Cause of order",
            Requirements = "Requirements of order",
            FacilityName = "Facility with Attachments",
            County = "Fulton",
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities[0].Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "ATT-1",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts[0].Id,
            ProposedOrderPostedDate = DateTime.Today,
            Attachment = new FormFile(Stream.Null, 0, 1, "test1", "test1.pdf"),
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        // Act
        var itemId = await repository.CreateAsync(resource);

        // Assert
        repositoryHelper.ClearChangeTracker();

        // Assert
        var order = await repository.GetAdminViewAsync(itemId);

        using (new AssertionScope())
        {
            order.Attachments.Count.Should().Be(1);
            var attachment = order.Attachments.Single();
            attachment.FileName.Should().Be("test1.pdf");
            attachment.FileExtension.Should().Be(".pdf");
            attachment.Size.Should().Be(1);
        }
    }
}
