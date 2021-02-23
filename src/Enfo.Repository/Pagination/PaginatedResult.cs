using System;
using System.Collections.Generic;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Pagination
{
    public class PaginatedResult<T>
    {
        public ICollection<T> Items { get; }
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public PaginatedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = Guard.NotNegative(totalCount, nameof(totalCount));
            PageNumber = Guard.Positive(pageNumber, nameof(pageNumber));
            PageSize = Guard.Positive(pageSize, nameof(pageSize));
            var itemsList = new List<T>();
            itemsList.AddRange(items);
            Items = itemsList;
        }

        public int TotalPages => (int) Math.Ceiling(TotalCount / (double) PageSize);
        public int CurrentCount => Items.Count;
        public int FirstItemIndex => Math.Min(PageSize * (PageNumber - 1) + 1, TotalCount);
        public int LastItemIndex => Math.Min(PageSize * PageNumber, TotalCount);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}