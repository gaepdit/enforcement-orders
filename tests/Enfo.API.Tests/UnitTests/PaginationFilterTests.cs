using Enfo.API.QueryStrings;
using FluentAssertions;
using Xunit;

namespace Enfo.API.Tests.UnitTests
{
    public class PaginationFilterTests
    {
        [Fact]
        public void PaginationFilterWithDefaultParams()
        {
            var paginationFilter = new PaginationFilter();

            var pagination = paginationFilter.Pagination();

            pagination.Take.Should().Be(PaginationFilter.DefaultPageSize);
            pagination.Skip.Should().Be(0);
            pagination.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void PaginationFilterWithSuppliedParams()
        {
            int pageSize = 10;
            int pageNumber = 2;

            var paginationFilter = new PaginationFilter
            {
                PageSize = pageSize,
                Page = pageNumber
            };

            var pagination = paginationFilter.Pagination();

            pagination.Take.Should().Be(pageSize);
            pagination.Skip.Should().Be((pageNumber - 1) * pageSize);
            pagination.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void PaginationFilterWithNegativePageSize()
        {
            var paginationFilter = new PaginationFilter
            {
                PageSize = -1
            };

            var pagination = paginationFilter.Pagination();

            pagination.Take.Should().Be(PaginationFilter.DefaultPageSize);
            pagination.Skip.Should().Be(0);
            pagination.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void PaginationFilterWithZeroPageNum()
        {
            var paginationFilter = new PaginationFilter
            {
                Page = 0
            };

            var pagination = paginationFilter.Pagination();

            pagination.Take.Should().Be(PaginationFilter.DefaultPageSize);
            pagination.Skip.Should().Be(0);
            pagination.IsPagingEnabled.Should().BeTrue();
        }
    }
}
