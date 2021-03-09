using System;
using System.Collections.Generic;
using Enfo.Repository.Specs;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources
{
    public class PaginatedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        
        public PaginatedResult([NotNull] IEnumerable<T> items, int totalCount, [NotNull] PaginationSpec paging)
        {
            Guard.NotNull(paging, nameof(paging));
            
            TotalCount = Guard.NotNegative(totalCount, nameof(totalCount));
            PageNumber = paging.PageNumber;
            PageSize = paging.PageSize;

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