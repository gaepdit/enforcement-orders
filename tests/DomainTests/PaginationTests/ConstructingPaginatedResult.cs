using Enfo.Domain.Pagination;

namespace EnfoTests.Domain.PaginationTests;

[TestFixture]
public class ConstructingPaginatedResult
{
    private readonly string[] _items = { "abc", "def" };

    [Test]
    public void ReturnsCorrectlyGivenCompleteList()
    {
        var itemCount = _items.Length;
        var result = new PaginatedResult<string>(_items, itemCount,
            new PaginationSpec(1, _items.Length));

        using (new AssertionScope())
        {
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
    }

    [Test]
    public void ReturnsCorrectlyGivenPartialList()
    {
        const int itemCount = 10;
        var result = new PaginatedResult<string>(_items, itemCount,
            new PaginationSpec(2, _items.Length));

        using (new AssertionScope())
        {
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
    }

    [Test]
    public void ThrowsExceptionGivenNegativeCount()
    {
        var action = () => new PaginatedResult<string>(_items, -1,
            new PaginationSpec(1, _items.Length));
        action.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be("totalCount");
    }

    [Test]
    public void ThrowsExceptionGivenZeroPageNum()
    {
        var action = () => new PaginatedResult<string>(_items, _items.Length,
            new PaginationSpec(0, _items.Length));
        action.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be("pageNumber");
    }

    [Test]
    public void ThrowsExceptionGivenZeroPageSize()
    {
        var action = () => new PaginatedResult<string>(_items, _items.Length,
            new PaginationSpec(1, 0));
        action.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be("pageSize");
    }
}
