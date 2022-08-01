using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Services;
using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.EnforcementOrderTests;

[TestFixture]
public class CreateTests
{
    [Test]
    public async Task FromValidItem_AddsNew()
    {
        EnforcementOrderCreate resource = new()
        {
            Cause = "Cause of order",
            Requirements = "Requirements of order",
            FacilityName = "Facility 1",
            County = "Fulton",
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities.First().Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-1",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        var expectedId = EnforcementOrderData.EnforcementOrders.Max(e => e.Id) + 1;
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var itemId = await repository.CreateAsync(resource);

        var enforcementOrder = new EnforcementOrder(resource) { Id = itemId };
        var expectedItem = new EnforcementOrderAdminView(enforcementOrder);

        var newItem = await repository.GetAdminViewAsync(itemId);

        Assert.Multiple(() =>
        {
            itemId.Should().Be(expectedId);
            newItem.Should().BeEquivalentTo(expectedItem);
        });
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
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
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
            Attachment = new FormFile( Stream.Null, 0, 2, "test2", "test2.pdf"),
        };

        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        // Act
        var itemId = await repository.CreateAsync(resource);

        // Assert
        var order = await repository.GetAdminViewAsync(itemId);

        Assert.Multiple(() =>
        {
            order.Attachments.Count.Should().Be(1);
            var attachment = order.Attachments.Single();
            attachment.FileName.Should().Be("test2.pdf");
            attachment.FileExtension.Should().Be(".pdf");
            attachment.Size.Should().Be(2);
        });
    }
}
