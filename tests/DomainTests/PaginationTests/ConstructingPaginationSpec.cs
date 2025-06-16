using Enfo.Domain.Pagination;

namespace EnfoTests.Domain.PaginationTests;

[TestFixture]
public class ConstructingPaginationSpec
{
    [Test]
    public void ReturnsWithPagingGivenPositiveInputs()
    {
        const int pageNumber = 2;
        const int pageSize = 10;

        var pagination = new PaginationSpec(pageNumber, pageSize);

        using (new AssertionScope())
        {
            pagination.PageSize.Should().Be(pageSize);
            pagination.PageNumber.Should().Be(pageNumber);
            pagination.Take.Should().Be(pageSize);
            pagination.Skip.Should().Be((pageNumber - 1) * pageSize);
        }
    }

    [Test]
    public void ThrowsExceptionGivenZeroPageSize()
    {
        const int pageNumber = 2;
        const int pageSize = 0;

        var action = () => new PaginationSpec(pageNumber, pageSize);
        action.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(nameof(pageSize));
    }

    [Test]
    public void ThrowsExceptionGivenZeroPageNum()
    {
        const int pageNumber = 0;
        const int pageSize = 20;

        var action = () => new PaginationSpec(pageNumber, pageSize);
        action.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(nameof(pageNumber));
    }
}
