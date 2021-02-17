using System;
using System.Collections.Generic;
using Enfo.API.QueryStrings;
using Enfo.Repository.Utils;

namespace Enfo.API.Classes
{
    public class PaginatedList<T>
    {
        public int TotalItems { get; }
        public int PageSize { get; }
        public int PageNumber { get; }

        public int TotalPages => PageSize == 0 ? 1 : (int)Math.Ceiling(TotalItems / (double)PageSize);
        public int FirstItemIndex => PageSize == 0 ? 1 : Math.Min(PageSize * (PageNumber - 1) + 1, TotalItems);
        public int LastItemIndex => PageSize == 0 ? TotalItems : Math.Min(PageSize * PageNumber, TotalItems);

        public bool HasPreviousPage => (PageNumber > 1);
        public bool HasNextPage => (PageNumber < TotalPages);

        public List<T> Items { get; } = new List<T>();

        public PaginatedList(
            IEnumerable<T> items,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            PageNumber = pageSize == 0 ? 1 : pageNumber;
            PageSize = pageSize;
            TotalItems = totalCount;

            Items.AddRange(items);
        }
    }

    public static class PaginatedListExtension
    {
        public static PaginatedList<T> GetPaginatedList<T>(
            this IEnumerable<T> items,
            int totalCount,
            PaginationFilter paging)
        {
            Guard.NotNull(paging, nameof(paging));
            return new PaginatedList<T>(items, totalCount, paging.Page, paging.PageSize);
        }
    }
}
