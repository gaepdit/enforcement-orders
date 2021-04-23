using System;
using System.Diagnostics.CodeAnalysis;
using Enfo.Domain.Specs;
using FluentAssertions;
using Xunit;

namespace EnfoTests.Domain.PaginationTests
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "CA1806")]
    public class ConstructingPaginationSpec
    {
        [Fact]
        public void ReturnsWithPagingGivenPositiveInputs()
        {
            const int pageNumber = 2;
            const int pageSize = 10;

            var pagination = new PaginationSpec(pageNumber, pageSize);

            pagination.PageSize.Should().Be(pageSize);
            pagination.PageNumber.Should().Be(pageNumber);
            pagination.Take.Should().Be(pageSize);
            pagination.Skip.Should().Be((pageNumber - 1) * pageSize);
        }

        [Fact]
        public void ThrowsExceptionGivenZeroPageSize()
        {
            const int pageNumber = 2;
            const int pageSize = 0;

            Action action = () => new PaginationSpec(pageNumber, pageSize);
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be(nameof(pageSize));
        }

        [Fact]
        public void ThrowsExceptionGivenZeroPageNum()
        {
            const int pageNumber = 0;
            const int pageSize = 20;

            Action action = () => new PaginationSpec(pageNumber, pageSize);
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be(nameof(pageNumber));
        }
    }
}