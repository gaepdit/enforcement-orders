using Enfo.Domain.Querying;
using System;

namespace Enfo.API.QueryStrings
{
    public class PaginationFilter
    {
        public const int DefaultPageSize = 20;

        public int PageSize { get; set; } = DefaultPageSize;
        public int Page { get; set; } = 1;

        public Pagination Pagination() =>
            Domain.Querying.Pagination.FromPageSizeAndNumber(
                pageSize: PageSize < 0 ? DefaultPageSize : PageSize,
                pageNum: Math.Max(Page, 1));
    }
}
