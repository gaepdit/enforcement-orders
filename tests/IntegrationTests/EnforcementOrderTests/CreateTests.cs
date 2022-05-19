using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class CreateTests
{
    [Test]
    public async Task CreateOrder_AddsNewItem()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        EnforcementOrderCreate sampleCreate = new()
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
        var newId = await repository.CreateAsync(sampleCreate);
        repositoryHelper.ClearChangeTracker();

        var order = new EnforcementOrder(sampleCreate) { Id = newId };
        var expected = new EnforcementOrderAdminView(ResourceHelper.FillNavigationProperties(order));

        var item = await repository.GetAdminViewAsync(newId);
        item.Should().BeEquivalentTo(expected);
    }
}
