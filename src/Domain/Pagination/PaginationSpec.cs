﻿namespace Enfo.Domain.Pagination;

public class PaginationSpec
{
    public int PageSize { get; }
    public int PageNumber { get; }

    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;

    public PaginationSpec(int pageNumber, int pageSize)
    {
        PageNumber = Guard.Positive(pageNumber);
        PageSize = Guard.Positive(pageSize);
    }
}
