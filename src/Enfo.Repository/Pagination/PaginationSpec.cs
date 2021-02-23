using Enfo.Repository.Utils;

namespace Enfo.Repository.Pagination
{
    public class PaginationSpec
    {
        public int Take { get; }
        public int Skip { get; }
        public bool IsPagingEnabled { get; }

        private PaginationSpec()
        {
            IsPagingEnabled = false;
        }

        private PaginationSpec(int skip, int take)
        {
            Guard.NotNegative(skip, nameof(skip));
            Guard.NotNegative(take, nameof(take));

            IsPagingEnabled = true;
            Take = take;
            Skip = skip;
        }

        public static PaginationSpec FromPageSizeAndNumber(int pageSize, int pageNumber)
        {
            Guard.NotNegative(pageSize, nameof(pageSize));
            Guard.Positive(pageNumber, nameof(pageNumber));

            // When page size is set to zero, return all results. (Page number is ignored.)
            return pageSize == 0 ? new PaginationSpec() : new PaginationSpec((pageNumber - 1) * pageSize, pageSize);
        }
    }
}