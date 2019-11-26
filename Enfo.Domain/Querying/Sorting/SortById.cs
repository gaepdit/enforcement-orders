using Enfo.Domain.Entities;

namespace Enfo.Domain.Querying
{
    public class SortById<T> : Sorting<T>
        where T : BaseEntity
    {
        public SortById(bool descending = false)
        {
            if (descending)
            {
                ApplyOrderByDescending(e => e.Id);
            }
            else
            {
                ApplyOrderBy(e => e.Id);
            }
        }
    }
}
