using Enfo.Domain.Pagination;

namespace Enfo.WebApp.Models;

public class PaginationNavModel
{
    public PaginationNavModel(IPaginatedResult paging, IDictionary<string, string> routeValues)
    {
        Paging = paging;
        RouteValues = routeValues;
    }

    public IPaginatedResult Paging { get; }
    public IDictionary<string, string> RouteValues { get; }
}
