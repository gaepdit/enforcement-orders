using Enfo.API.QueryStrings;
using FluentAssertions;
using System;
using Xunit;

namespace Enfo.API.Tests
{
    public class PaginationFilterTests
    {
        private const int defaultPageSize = 20;
        private const int defaultPageNumber = 1;

        [Fact]
        public void PaginationFilterWithDefaultParams()
        {
            var paginationFilter = new PaginationFilter();

            var pagination = paginationFilter.Pagination();

            pagination.Take.Should().Be(defaultPageSize);
            pagination.Skip.Should().Be((defaultPageNumber - 1) * defaultPageSize);
            pagination.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void PaginationFilterWithSuppliedParams()
        {
            int pageSize = 10;
            int pageNumber = 2;

            var paginationFilter = new PaginationFilter();
            paginationFilter.PageSize = pageSize;
            paginationFilter.Page = pageNumber;

            var pagination = paginationFilter.Pagination();

            pagination.Take.Should().Be(pageSize);
            pagination.Skip.Should().Be((pageNumber - 1) * pageSize);
            pagination.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void PaginationFilterWithNegativePageSize()
        {
            var paginationFilter = new PaginationFilter();
            paginationFilter.PageSize = -1;
            paginationFilter.Page = defaultPageNumber;

            var pagination = paginationFilter.Pagination();

            pagination.Take.Should().Be(defaultPageSize);
            pagination.Skip.Should().Be((defaultPageNumber - 1) * defaultPageSize);
            pagination.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void PaginationFilterWithZeroPageNum()
        {
            var paginationFilter = new PaginationFilter();
            paginationFilter.PageSize = defaultPageSize;
            paginationFilter.Page = 0;

            var pagination = paginationFilter.Pagination();

            pagination.Take.Should().Be(defaultPageSize);
            pagination.Skip.Should().Be((defaultPageNumber - 1) * defaultPageSize);
            pagination.IsPagingEnabled.Should().BeTrue();
        }
    }
}
