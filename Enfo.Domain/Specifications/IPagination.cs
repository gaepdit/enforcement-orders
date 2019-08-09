namespace Enfo.Domain.Specifications
{
    public interface IPagination
    {
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
