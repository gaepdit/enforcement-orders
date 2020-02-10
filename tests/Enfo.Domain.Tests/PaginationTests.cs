using Enfo.Domain.Querying;
using FluentAssertions;
using System;
using Xunit;

namespace Enfo.Domain.Tests
{
    public class PaginationTests
    {
        [Fact]
        public void GetPaginatedReturnsCorrectly()
        {
            int pageSize = 10;
            int pageNum = 2;
            
            var pagination = Pagination.FromPageSizeAndNumber(pageSize, pageNum);

            pagination.Take.Should().Be(pageSize);
            pagination.Skip.Should().Be((pageNum - 1) * pageSize);
            pagination.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void GetPaginatedWithZeroPageSizeReturnsCorrectly()
        {
            int pageSize = 0;
            int pageNum = 2;

            var pagination = Pagination.FromPageSizeAndNumber(pageSize, pageNum);

            pagination.Take.Should().Be(0);
            pagination.Skip.Should().Be(0);
            pagination.IsPagingEnabled.Should().BeFalse();
        }

        [Fact]
        public void GetPaginatedWithNegativePageSizeThrowsException()
        {
            int pageSize = -1;
            int pageNum = 2;

            Action act = () => Pagination.FromPageSizeAndNumber(pageSize, pageNum);
            act.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be(nameof(pageSize));
        }

        [Fact]
        public void GetPaginatedWithZeroPageNumThrowsException()
        {
            int pageSize = 20;
            int pageNum = 0;

            Action act = () => Pagination.FromPageSizeAndNumber(pageSize, pageNum);
            act.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be(nameof(pageNum));
        }
    }
}
