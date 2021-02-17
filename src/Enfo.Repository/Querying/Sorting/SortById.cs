using Enfo.Domain.Entities;

namespace Enfo.Repository.Querying
{
    public class SortById<T> : Sorting<T>
        where T : BaseEntity
    {
        public SortById(SortDirection sortDirection = SortDirection.Ascending)
        {
            if (sortDirection == SortDirection.Ascending)
            {
                ApplyOrderBy(e => e.Id);
            }
            else
            {
                ApplyOrderByDescending(e => e.Id);
            }
        }
    }
}
