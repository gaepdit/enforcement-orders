using Enfo.Domain.Pagination;
using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace EnfoTests.Domain.PaginationTests
{
    public class ConstructingPaginatedResult
    {
        private readonly string[] _items = {"abc", "def"};

        [Fact]
        public void ReturnsCorrectlyGivenCompleteList()
        {
            var itemCount = _items.Length;
            var result = new PaginatedResult<string>(_items, itemCount,
                new PaginationSpec(1, _items.Length));

            result.CurrentCount.Should().Be(_items.Length);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(_items.Length);
            result.TotalCount.Should().Be(itemCount);
            result.TotalPages.Should().Be(1);
            result.FirstItemIndex.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
            result.LastItemIndex.Should().Be(2);
        }

        [Fact]
        public void ReturnsCorrectlyGivenPartialList()
        {
            const int itemCount = 10;
            var result = new PaginatedResult<string>(_items, itemCount,
                new PaginationSpec(2, _items.Length));

            result.CurrentCount.Should().Be(_items.Length);
            result.PageNumber.Should().Be(2);
            result.PageSize.Should().Be(_items.Length);
            result.TotalCount.Should().Be(itemCount);
            result.TotalPages.Should().Be(5);
            result.FirstItemIndex.Should().Be(3);
            result.HasNextPage.Should().BeTrue();
            result.HasPreviousPage.Should().BeTrue();
            result.LastItemIndex.Should().Be(4);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "CA1806")]
        public void ThrowsExceptionGivenNegativeCount()
        {
            Action action = () => new PaginatedResult<string>(_items, -1,
                new PaginationSpec(1, _items.Length));
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be("totalCount");
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "CA1806")]
        public void ThrowsExceptionGivenZeroPageNum()
        {
            Action action = () => new PaginatedResult<string>(_items, _items.Length,
                new PaginationSpec(0, _items.Length));
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be("pageNumber");
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "CA1806")]
        public void ThrowsExceptionGivenZeroPageSize()
        {
            Action action = () => new PaginatedResult<string>(_items, _items.Length,
                new PaginationSpec(1, 0));
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be("pageSize");
        }
    }
}