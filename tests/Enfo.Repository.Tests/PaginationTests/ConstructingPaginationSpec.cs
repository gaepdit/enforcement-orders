using System;
using System.Diagnostics.CodeAnalysis;
using Enfo.Repository.Specs;
using FluentAssertions;
using Xunit;

namespace Enfo.Repository.Tests.PaginationTests
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "CA1806")]
    public class ConstructingPaginationSpec
    {
        [Fact]
        public void ReturnsDefaultPaging()
        {
            var pagination = new PaginationSpec();

            pagination.PageSize.Should().Be(20);
            pagination.PageNumber.Should().Be(1);
            pagination.Take.Should().Be(20);
            pagination.Skip.Should().Be(0);
        }

        [Fact]
        public void ReturnsWithPagingGivenPositiveInputs()
        {
            const int pageSize = 10;
            const int pageNumber = 2;

            var pagination = new PaginationSpec(pageNumber, pageSize);

            pagination.PageSize.Should().Be(pageSize);
            pagination.PageNumber.Should().Be(pageNumber);
            pagination.Take.Should().Be(pageSize);
            pagination.Skip.Should().Be((pageNumber - 1) * pageSize);
        }

        [Fact]
        public void ThrowsExceptionGivenZeroPageSize()
        {
            const int pageSize = 0;
            const int pageNumber = 2;

            Action action = () => new PaginationSpec(pageNumber, pageSize);
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be(nameof(pageSize));
        }

        [Fact]
        public void ThrowsExceptionGivenZeroPageNum()
        {
            const int pageSize = 20;
            const int pageNumber = 0;

            Action action = () => new PaginationSpec(pageNumber, pageSize);
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be(nameof(pageNumber));
        }
    }
}