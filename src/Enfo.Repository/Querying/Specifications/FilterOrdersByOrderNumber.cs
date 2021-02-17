using Enfo.Domain.Entities;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Querying
{
    public class FilterOrdersByOrderNumber : Specification<EnforcementOrder>
    {
        public FilterOrdersByOrderNumber(string orderNumber)
        {
            Guard.NotNullOrWhiteSpace(orderNumber, nameof(orderNumber));
            ApplyCriteria(e => e.OrderNumber.ToLower().Contains(orderNumber.ToLower()));
        }
    }
}
