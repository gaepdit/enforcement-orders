namespace Enfo.Domain.Pagination
{
    public interface IPagination
    {
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
