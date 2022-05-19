using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Services;
using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.EnforcementOrderTests;

[TestFixture]
public class CreateTests
{
    [Test]
    public async Task FromValidItem_AddsNew()
    {
        // Sample data for create
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
}
