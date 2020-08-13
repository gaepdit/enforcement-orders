using Enfo.API.Classes;
using Enfo.API.QueryStrings;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Enfo.API.Tests.UnitTests
{
    public class PaginatedListTests
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { new List<int> { 1 } },
                new object[] { new List<int> { 1, 2, 3 } },
                new object[] { new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } }
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void DefaultPaginatedListAndFilter(List<int> items)
        {
            var paginationFilter = new PaginationFilter();
            var paginatedList = items.GetPaginatedList(items.Count, paginationFilter);

            paginatedList.FirstItemIndex.Should().Be(1);
            paginatedList.LastItemIndex.Should().Be(items.Count);
            paginatedList.HasNextPage.Should().Be(false);
            paginatedList.HasPreviousPage.Should().Be(false);
            paginatedList.Items.Should().BeEquivalentTo(items);
            paginatedList.PageNumber.Should().Be(1);
            paginatedList.PageSize.Should().Be(PaginationFilter.DefaultPageSize);
            paginatedList.TotalItems.Should().Be(items.Count);
            paginatedList.TotalPages.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void PaginatedListWithSuppliedParams(List<int> items)
        {
            int pageNumber = 2;

            var paginationFilter = new PaginationFilter
            {
                PageSize = items.Count,
                Page = pageNumber
            };

            int totalCount = 20;
            var paginatedList = items.GetPaginatedList(totalCount, paginationFilter);

            paginatedList.FirstItemIndex.Should().Be(items.Count + 1);
            paginatedList.LastItemIndex.Should().Be(items.Count * pageNumber);
            paginatedList.HasNextPage.Should().Be(items.Count * pageNumber < totalCount);
            paginatedList.HasPreviousPage.Should().Be(true);
            paginatedList.Items.Should().BeEquivalentTo(items);
            paginatedList.PageNumber.Should().Be(2);
            paginatedList.PageSize.Should().Be(items.Count);
            paginatedList.TotalItems.Should().Be(totalCount);
            paginatedList.TotalPages.Should().Be((int)Math.Ceiling(totalCount / (double)items.Count));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void PaginatedListWithNegativePageSize(List<int> items)
        {
            var paginationFilter = new PaginationFilter
            {
                PageSize = -1
            };

            var paginatedList = items.GetPaginatedList(items.Count, paginationFilter);

            paginatedList.FirstItemIndex.Should().Be(1);
            paginatedList.LastItemIndex.Should().Be(items.Count);
            paginatedList.HasNextPage.Should().Be(false);
            paginatedList.HasPreviousPage.Should().Be(false);
            paginatedList.Items.Should().BeEquivalentTo(items);
            paginatedList.PageNumber.Should().Be(1);
            paginatedList.PageSize.Should().Be(PaginationFilter.DefaultPageSize);
            paginatedList.TotalItems.Should().Be(items.Count);
            paginatedList.TotalPages.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void PaginatedListWithZeroPageNum(List<int> items)
        {
            var paginationFilter = new PaginationFilter
            {
                Page = 0
            };

            var paginatedList = items.GetPaginatedList(items.Count, paginationFilter);

            paginatedList.FirstItemIndex.Should().Be(1);
            paginatedList.LastItemIndex.Should().Be(items.Count);
            paginatedList.HasNextPage.Should().Be(false);
            paginatedList.HasPreviousPage.Should().Be(false);
            paginatedList.Items.Should().BeEquivalentTo(items);
            paginatedList.PageNumber.Should().Be(1);
            paginatedList.PageSize.Should().Be(PaginationFilter.DefaultPageSize);
            paginatedList.TotalItems.Should().Be(items.Count);
            paginatedList.TotalPages.Should().Be(1);
        }
    }
}
