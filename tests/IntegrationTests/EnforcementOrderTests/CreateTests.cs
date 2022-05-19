using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities.First().Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-4",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
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
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities.First().Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-2",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        var action = async () =>
        {
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(EnforcementOrderCreate.County));
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
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities.First().Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "ATT-1",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        resource.Attachments.AddRange(new[]
        {
            new FormFile(Stream.Null, 0, 1, "test1", "test1.pdf"),
            new FormFile(Stream.Null, 0, 2, "test2", "test2.pdf"),
        });

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        // Act
        var itemId = await repository.CreateAsync(resource);

        // Assert
        repositoryHelper.ClearChangeTracker();

        // Assert
        var order = await repository.GetAdminViewAsync(itemId);

        Assert.Multiple(() =>
        {
            order.Attachments.Count.Should().Be(2);
            var attachment = order.Attachments.OrderBy(a => a.FileName).Last();
            attachment.FileName.Should().Be("test2.pdf");
            attachment.FileExtension.Should().Be(".pdf");
            attachment.Size.Should().Be(2);
        });
    }
}
