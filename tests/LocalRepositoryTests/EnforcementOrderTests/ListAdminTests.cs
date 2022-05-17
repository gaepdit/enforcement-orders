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
public class ListAdminTests
{
    [Test]
    public async Task ByDefault_ReturnsNonDeleted()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAdminAsync(new EnforcementOrderAdminSpec(), new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => !e.Deleted)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.PageNumber.Should().Be(1);
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        });
    }

    [Test]
    public async Task WithShowDeletedSpec_ReturnsOnlyDeleted()
    {
        var spec = new EnforcementOrderAdminSpec { ShowDeleted = true };
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => e.Deleted)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
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
        var spec = new EnforcementOrderAdminSpec
        {
            Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName,
        };
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => !e.Deleted)
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.PageNumber.Should().Be(1);
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        });
    }

    [Test]
    public async Task WithUnmatchedFacilityNameSpec_ReturnsEmptyResult()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Facility = "None",
        };
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));
        
        Assert.Multiple(() =>
        {
            result.PageNumber.Should().Be(1);
            result.CurrentCount.Should().Be(0);
            result.Items.Count.Should().Be(0);
        });
    }
    [Test]
    public async Task WithTextMatchSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Text = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Cause[..4].ToLowerInvariant(),
        };
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => !e.Deleted)
            .Where(e => e.Cause != null && e.Cause.Contains(spec.Text) ||
                e.Requirements != null && e.Requirements.Contains(spec.Text))
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.PageNumber.Should().Be(1);
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().BeEquivalentTo(expectedList);
        });
    }

}
