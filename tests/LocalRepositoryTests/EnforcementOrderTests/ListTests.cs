using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.Domain.Services;
using Enfo.LocalRepository;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestData;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class ListTests
{
    [Test]
    public async Task ByDefault_ReturnsOnlyPublic()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAsync(new EnforcementOrderSpec(), new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => e.GetIsPublic)
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.PageNumber.Should().Be(1);
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        });
    }

    [Test]
    public async Task WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName,
        };
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => !e.Deleted && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.PageNumber.Should().Be(1);
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        });
    }

    // Tests confirming date range processing

    [Test]
    public async Task WithDateRangeBetweenProposedAndExecutedDates_ReturnsNone()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 3, 1),
            TillDate = new DateTime(999, 4, 1),
        };
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(0);
            result.Items.Count.Should().Be(0);
            result.PageNumber.Should().Be(1);
        });
    }

    [Test]
    public async Task WithStartDateBeforeFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 1, 1),
        };
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Count.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
            result.PageNumber.Should().Be(1);
        });
    }

    [Test]
    public async Task WithEndDateAfterFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            TillDate = new DateTime(999, 6, 1),
        };
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Count.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
            result.PageNumber.Should().Be(1);
        });
    }
}
