using EfRepositoryTests.Helpers;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;

namespace EfRepositoryTests.EnforcementOrderTests;

public class ListTests
{
    [Test]
    public async Task List_ByDefault_ReturnsOnlyPublic()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        var result = await repository.ListAsync(new EnforcementOrderSpec(), new PaginationSpec(1, 50));

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.Items.Should().HaveSameCount(EnforcementOrderData.EnforcementOrders.Where(e => e.GetIsPublic));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => e.GetIsPublic).Id));
        }
    }

    [Test]
    public async Task List_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName,
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e =>
                !e.Deleted
                && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderSummaryView(expectedList[0].Id));
        }
    }

    [Test]
    public async Task List_WithDateRangeBetweenProposedAndExecutedDates_ReturnsNone()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 3, 1, 0, 0, 0, DateTimeKind.Local),
            TillDate = new DateTime(999, 4, 1, 0, 0, 0, DateTimeKind.Local),
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(0);
            result.Items.Count.Should().Be(0);
            result.PageNumber.Should().Be(1);
        }
    }

    [Test]
    public async Task List_WithStartDateBeforeFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 1, 1, 0, 0, 0, DateTimeKind.Local),
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.PageNumber.Should().Be(1);

            var resultList = result.Items.ToList();
            resultList.Should().HaveCount(expectedList.Count);
            resultList.Should().BeEquivalentTo(expectedList);
        }
    }

    [Test]
    public async Task List_WithEndDateAfterFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            TillDate = new DateTime(999, 6, 1, 0, 0, 0, DateTimeKind.Local),
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.PageNumber.Should().Be(1);

            var resultList = result.Items.ToList();
            resultList.Should().HaveCount(expectedList.Count);
            resultList.Should().BeEquivalentTo(expectedList);
        }
    }

    [Test]
    public async Task ListDetailed_ByDefault_ReturnsOnlyPublic()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(new EnforcementOrderSpec(), new PaginationSpec(1, 50));

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.Items.Should().HaveSameCount(EnforcementOrderData.EnforcementOrders.Where(e => e.GetIsPublic));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderDetailedView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => e.GetIsPublic).Id));
        }
    }

    [Test]
    public async Task ListDetailed_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderSpec
            { Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e =>
                !e.Deleted
                && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderDetailedView(expectedList[0].Id));
        }
    }

    [Test]
    public async Task ListDetailed_WithDateRangeBetweenProposedAndExecutedDates_ReturnsNone()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 3, 1, 0, 0, 0, DateTimeKind.Local),
            TillDate = new DateTime(999, 4, 1, 0, 0, 0, DateTimeKind.Local),
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 50));

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(0);
            result.Items.Should().BeEmpty();
            result.PageNumber.Should().Be(1);
        }
    }

    [Test]
    public async Task ListDetailed_WithStartDateBeforeFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 1, 1, 0, 0, 0, DateTimeKind.Local),
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderDetailedView(expectedList[0].Id));
        }
    }

    [Test]
    public async Task ListDetailed_WithEndDateAfterFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            TillDate = new DateTime(999, 6, 1, 0, 0, 0, DateTimeKind.Local),
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderDetailedView(expectedList[0].Id));
        }
    }

    [Test]
    public async Task ListAdmin_ByDefault_ReturnsNonDeleted()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(new EnforcementOrderAdminSpec(), new PaginationSpec(1, 50));

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => !e.Deleted));
            result.Items.Should().HaveSameCount(EnforcementOrderData.EnforcementOrders.Where(e => !e.Deleted));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => !e.Deleted).Id));
        }
    }

    [Test]
    public async Task ListAdmin_WithSpecShowDeleted_ReturnsOnlyDeleted()
    {
        var spec = new EnforcementOrderAdminSpec { ShowDeleted = true };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 50));

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => e.Deleted));
            result.Items.Should().HaveSameCount(EnforcementOrderData.EnforcementOrders.Where(e => e.Deleted));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => e.Deleted).Id));
        }
    }

    [Test]
    public async Task ListAdmin_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName,
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e => !e.Deleted
                        && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(expectedList[0].Id));
        }
    }

    [Test]
    public async Task ListAdmin_WithUnmatchedFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Facility = "none",
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 50));

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(0);
            result.Items.Count.Should().Be(0);
            result.PageNumber.Should().Be(1);
        }
    }

    [Test]
    public async Task ListAdmin_WithTextMatchSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Text = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Cause[..4].ToLowerInvariant(),
        };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 50));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e => !e.Deleted)
            .Where(e => e.Cause != null && e.Cause.Contains(spec.Text) ||
                        e.Requirements != null && e.Requirements.Contains(spec.Text))
            .ToList();

        using (new AssertionScope())
        {
            result.TotalCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(expectedList[0].Id));
        }
    }
}
