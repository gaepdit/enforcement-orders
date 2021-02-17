namespace Enfo.Repository.Querying
{
    public interface IPagination
    {
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
