using Enfo.Domain.Utils;

namespace Enfo.Domain.Querying
{
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

        //public static Pagination FromPageSkipAndTake(int skip, int take)
        //{
        //    Check.NotNegative(skip, nameof(skip));
        //    Check.Positive(take, nameof(take));

        //    return new Pagination(skip, take);
        //}

        public static Pagination FromPageSizeAndNumber(int pageSize, int pageNumber)
        {
            Guard.NotNegative(pageSize, nameof(pageSize));
            Guard.Positive(pageNumber, nameof(pageNumber));

            // When page size is set to zero, return all results. (Page number is ignored.)
            if (pageSize == 0)
            {
                return new Pagination();
            }

            return new Pagination((pageNumber - 1) * pageSize, pageSize);
        }
    }
}
