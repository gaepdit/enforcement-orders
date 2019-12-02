using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByOrderNumber : Specification<EnforcementOrder>
    {
        public FilterOrdersByOrderNumber(string orderNumber)
        {
            Check.NotNullOrWhiteSpace(orderNumber, nameof(orderNumber));
            ApplyCriteria(e => e.OrderNumber.ToLower().Contains(orderNumber.ToLower()));
        }
    }
}
