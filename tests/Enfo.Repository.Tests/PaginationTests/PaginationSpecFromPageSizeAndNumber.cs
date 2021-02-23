using System;
using Enfo.Repository.Pagination;
using FluentAssertions;
using Xunit;

namespace Enfo.Repository.Tests.PaginationTests
{
    public class PaginationSpecFromPageSizeAndNumber
    {
        [Fact]
        public void ReturnsWithPagingGivenPositiveInputs()
        {
            const int pageSize = 10;
            const int pageNumber = 2;

            var pagination = PaginationSpec.FromPageSizeAndNumber(pageSize, pageNumber);

            pagination.Take.Should().Be(pageSize);
            pagination.Skip.Should().Be((pageNumber - 1) * pageSize);
            pagination.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void ReturnsDisabledPagingGivenZeroPageSize()
        {
            const int pageSize = 0;
            const int pageNumber = 2;

            var pagination = PaginationSpec.FromPageSizeAndNumber(pageSize, pageNumber);

            pagination.Take.Should().Be(0);
            pagination.Skip.Should().Be(0);
            pagination.IsPagingEnabled.Should().BeFalse();
        }

        [Fact]
        public void ThrowsExceptionGivenNegativePageSize()
        {
            const int pageSize = -1;
            const int pageNumber = 2;

            Action action = () => PaginationSpec.FromPageSizeAndNumber(pageSize, pageNumber);
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be(nameof(pageSize));
        }

        [Fact]
        public void ThrowsExceptionGivenZeroPageNum()
        {
            const int pageSize = 20;
            const int pageNumber = 0;

            Action action = () => PaginationSpec.FromPageSizeAndNumber(pageSize, pageNumber);
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be(nameof(pageNumber));
        }
    }
}