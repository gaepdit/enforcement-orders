using Enfo.Domain.Querying;
using System;

namespace Enfo.API
{
    public static class ApiPagination
    {
        public const int DefaultPageSize = 20;

        public static Pagination Paginate(int pageSize, int pageNum)
        {
            return Pagination.FromPageSizeAndNumber(pageSize < 0 ? DefaultPageSize : pageSize, Math.Max(pageNum, 1));
        }
    }
}
