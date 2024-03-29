﻿namespace Enfo.Domain.Pagination;

public class PaginatedResult<T> : IPaginatedResult
{
    public PaginatedResult([NotNull] IEnumerable<T> items, int totalCount, [NotNull] PaginationSpec paging)
    {
        Guard.NotNull(paging);

        TotalCount = Guard.NotNegative(totalCount);
        PageNumber = paging.PageNumber;
        PageSize = paging.PageSize;

        var itemsList = new List<T>();
        itemsList.AddRange(items);
        Items = itemsList;
    }

    public int TotalCount { get; }
    public int PageSize { get; }
    public int PageNumber { get; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int CurrentCount => Items.Count;

    [JsonIgnore]
    public int FirstItemIndex => Math.Min(PageSize * (PageNumber - 1) + 1, TotalCount);
    [JsonIgnore]
    public int LastItemIndex => Math.Min(PageSize * PageNumber, TotalCount);
    [JsonIgnore]
    public bool HasPreviousPage => PageNumber > 1;
    [JsonIgnore]
    public bool HasNextPage => PageNumber < TotalPages;

    public IList<T> Items { get; }
}

public interface IPaginatedResult
{
    int TotalCount { get; }
    int PageNumber { get; }
    int TotalPages { get; }
    int CurrentCount { get; }
    int FirstItemIndex { get; }
    int LastItemIndex { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}
