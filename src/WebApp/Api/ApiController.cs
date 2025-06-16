using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using Microsoft.AspNetCore.Mvc;

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
        var baseUrl = config["BaseUrl"];
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
        var baseUrl = config["BaseUrl"];
        var item = await order.GetAsync(id);
        return item != null
            ? Ok(new EnforcementOrderApiView(item, baseUrl))
            : Problem("ID not found.", statusCode: 404);
    }

    [HttpGet(nameof(LegalAuthority))]
    public Task<IReadOnlyList<LegalAuthorityView>> ListLegalAuthoritiesAsync(
        [FromServices] ILegalAuthorityRepository legalAuthority,
        [FromQuery] bool includeInactive = false) =>
        legalAuthority.ListAsync(includeInactive);

    [HttpGet(nameof(LegalAuthority) + "/{id:int}")]
    public async Task<ActionResult<LegalAuthorityView>> GetLegalAuthorityAsync(
        [FromServices] ILegalAuthorityRepository legalAuthority,
        [FromRoute] int id)
    {
        var item = await legalAuthority.GetAsync(id);
        return item != null ? Ok(item) : Problem("ID not found.", statusCode: 404);
    }
}
