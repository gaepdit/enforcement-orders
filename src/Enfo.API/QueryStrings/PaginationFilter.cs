using System;
using System.ComponentModel;
using Enfo.Domain.Querying;

namespace Enfo.API.QueryStrings
{
    public class PaginationFilter
    {
        public const int DefaultPageSize = 20;
        private int pageSize = DefaultPageSize;
        private int page = 1;

        [DefaultValue(DefaultPageSize)]
        public int PageSize { get => pageSize; set => pageSize = value < 0 ? DefaultPageSize : value; }

        [DefaultValue(1)]
        public int Page { get => page; set => page = Math.Max(value, 1); }

        public Pagination Pagination() =>
            Domain.Querying.Pagination.FromPageSizeAndNumber(pageSize, page);
    }
}
