using System;
using System.ComponentModel;
using Enfo.Domain.Querying;

namespace Enfo.API.QueryStrings
{
    public class PaginationFilter
    {
        public const int DefaultPageSize = 20;

        [DefaultValue(DefaultPageSize)]
        public int PageSize { get; set; } = DefaultPageSize;

        [DefaultValue(1)]
        public int Page { get; set; } = 1;

        public Pagination Pagination() =>
            Domain.Querying.Pagination.FromPageSizeAndNumber(
                pageSize: PageSize < 0 ? DefaultPageSize : PageSize,
                pageNum: Math.Max(Page, 1));
    }
}
