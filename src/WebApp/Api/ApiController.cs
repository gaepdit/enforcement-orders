using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;

namespace Enfo.WebApp.Api;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class ApiController : ControllerBase
{
    [HttpGet(nameof(EnforcementOrder))]
    public async Task<PaginatedResult<EnforcementOrderApiView>> ListOrdersAsync(
        [FromServices] IEnforcementOrderRepository order,
        [FromServices] IConfiguration config,
        [FromQuery] EnforcementOrderSpec spec,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25)
    {
        var baseUrl = config["BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
        var paging = new PaginationSpec(page, pageSize);
        var list = await order.ListDetailedAsync(spec, paging);

        return new PaginatedResult<EnforcementOrderApiView>(
            list.Items.Select(e => new EnforcementOrderApiView(e, baseUrl)),
            list.TotalCount, paging);
    }

    [HttpGet(nameof(EnforcementOrder) + "/{id:int}")]
    public async Task<ActionResult<EnforcementOrderApiView>> GetOrderAsync(
        [FromServices] IEnforcementOrderRepository order,
        [FromServices] IConfiguration config,
        [FromRoute] int id)
    {
        var baseUrl = config["BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
        var item = await order.GetAsync(id);
        return item != null
            ? Ok(new EnforcementOrderApiView(item, baseUrl))
            : Problem("ID not found.", statusCode: 404);
    }

    [HttpGet(nameof(LegalAuthority))]
    public Task<IReadOnlyList<LegalAuthorityViewDto>> ListLegalAuthoritiesAsync(
        [FromServices] ILegalAuthorityRepository legalAuthority,
        [FromQuery] bool includeInactive = false) =>
        legalAuthority.ListAsync(includeInactive);

    [HttpGet(nameof(LegalAuthority) + "/{id:int}")]
    public async Task<ActionResult<LegalAuthorityViewDto>> GetLegalAuthorityAsync(
        [FromServices] ILegalAuthorityRepository legalAuthority,
        [FromRoute] int id, CancellationToken token = default)
    {
        var item = await legalAuthority.GetAsync(id, token);
        return item != null ? Ok(item) : Problem("ID not found.", statusCode: 404);
    }
}
