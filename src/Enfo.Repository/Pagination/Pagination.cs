using Enfo.Repository.Utils;

namespace Enfo.Repository.Querying
{
    public interface IPagination
    {
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }

    public class Pagination : IPagination
    {
        public int Take { get; }
        public int Skip { get; }
        public bool IsPagingEnabled { get; }

        private Pagination()
        {
            IsPagingEnabled = false;
        }

        private Pagination(int skip, int take)
        {
            IsPagingEnabled = true;
            Take = take;
            Skip = skip;
        }

        public static Pagination FromPageSizeAndNumber(int pageSize, int pageNumber)
        {
            Guard.NotNegative(pageSize, nameof(pageSize));
            Guard.Positive(pageNumber, nameof(pageNumber));

            // When page size is set to zero, return all results. (Page number is ignored.)
            return pageSize == 0 ? new Pagination() : new Pagination((pageNumber - 1) * pageSize, pageSize);
        }
    }
}