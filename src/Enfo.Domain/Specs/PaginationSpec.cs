using Enfo.Domain.Utils;

namespace Enfo.Domain.Specs;

public class PaginationSpec
{
    public int PageSize { get; }
    public int PageNumber { get; }

    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;

    public PaginationSpec(int pageNumber, int pageSize)
    {
        PageNumber = Guard.Positive(pageNumber, nameof(pageNumber));
        PageSize = Guard.Positive(pageSize, nameof(pageSize));
    }
}
