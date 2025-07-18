using Enfo.Domain.Pagination;

namespace Enfo.WebApp.Models;

public record PaginationNavModel(IPaginatedResult Paging, IDictionary<string, string> RouteValues);
