using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.LocalRepository.EnforcementOrders;
using Enfo.LocalRepository.EpdContacts;
using Enfo.LocalRepository.LegalAuthorities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class CreateTests
{
    // Sample data for create
    private readonly EnforcementOrderCreate _resource = new()
    {
        Cause = "Cause of order",
        Requirements = "Requirements of order",
        FacilityName = "abc",
        County = "Fulton",
        LegalAuthorityId = LegalAuthorityData.LegalAuthorities.First().Id,
        Progress = PublicationProgress.Draft,
        OrderNumber = "NEW-1",
        CreateAs = NewEnforcementOrderType.Proposed,
        CommentPeriodClosesDate = DateTime.Today.AddDays(1),
        CommentContactId = EpdContactData.EpdContacts.First().Id,
        ProposedOrderPostedDate = DateTime.Today,
    };

    [Test]
    public async Task FromValidItem_AddsNew()
    {
        var expectedId = EnforcementOrderData.EnforcementOrders.Max(e => e.Id) + 1;
        using var repository = new EnforcementOrderRepository();

        var itemId = await repository.CreateAsync(_resource);

        var enforcementOrder = new EnforcementOrder(_resource) { Id = itemId };
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
            FacilityName = "abc",
            County = null,
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities.First().Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-1",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(EnforcementOrderCreate.County));
    }
}
