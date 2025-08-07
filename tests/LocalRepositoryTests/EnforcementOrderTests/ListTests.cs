using Enfo.Domain.Attachments;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.LocalRepository.Repositories;

namespace LocalRepositoryTests.EnforcementOrderTests;

[TestFixture]
public class ListTests
{
    [Test]
    public async Task ByDefault_ReturnsOnlyPublic()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListAsync(new EnforcementOrderSpec(), new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => e.GetIsPublic)
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        using (new AssertionScope())
        {
            result.PageNumber.Should().Be(1);
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        }
    }

    [Test]
    public async Task WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName,
        };
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => !e.Deleted && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        using (new AssertionScope())
        {
            result.PageNumber.Should().Be(1);
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        }
    }

    [Test]
    public async Task WithFacilityNameSpec_AndDifferentCase_ReturnsCaseInsensitiveMatches()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName.ToUpper(),
        };
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => !e.Deleted && string.Equals(e.FacilityName.ToLower(), spec.Facility.ToLower()))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        using (new AssertionScope())
        {
            result.PageNumber.Should().Be(1);
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        }
    }

    [Test]
    public async Task WithOrderNumberSpec_AndDifferentCase_ReturnsCaseInsensitiveMatches()
    {
        var spec = new EnforcementOrderSpec
        {
            OrderNumber = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).OrderNumber.ToLower(),
        };

        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => !e.Deleted && string.Equals(e.OrderNumber.ToLower(), spec.OrderNumber.ToLower()))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        using (new AssertionScope())
        {
            result.PageNumber.Should().Be(1);
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        }
    }

    // Tests confirming date range processing

    [Test]
    public async Task WithDateRangeBetweenProposedAndExecutedDates_ReturnsNone()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 3, 1,0,0,0,DateTimeKind.Local),
            TillDate = new DateTime(999, 4, 1,0,0,0,DateTimeKind.Local),
        };
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(0);
            result.Items.Count.Should().Be(0);
            result.PageNumber.Should().Be(1);
        }
    }

    [Test]
    public async Task WithStartDateBeforeFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 1, 1,0,0,0,DateTimeKind.Local),
        };
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Count.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
            result.PageNumber.Should().Be(1);
        }
    }

    [Test]
    public async Task WithEndDateAfterFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            TillDate = new DateTime(999, 6, 1,0,0,0,DateTimeKind.Local),
        };
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Count.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
            result.PageNumber.Should().Be(1);
        }
    }
}
