namespace Enfo.Domain.Pagination
{
    public class Pagination : IPagination
    {
        public int Take { get; }
        public int Skip { get; }
        public bool IsPagingEnabled { get; } = false;

        public Pagination() { }

        private Pagination(int skip, int take)
        {
            IsPagingEnabled = true;
            Take = take;
            Skip = skip;
        }

        public static Pagination FromPageSkipAndTake(int skip, int take)
        {
            return new Pagination(skip, take);
        }

        public static Pagination FromPageSizeAndIndex(int pageSize, int pageIndex)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                return new Pagination();
            }

            return new Pagination((pageIndex - 1) * pageSize, pageSize);
        }
    }
}
