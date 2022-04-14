﻿using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.WebApp.Api
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        [HttpGet(nameof(EnforcementOrder))]
        public Task<PaginatedResult<EnforcementOrderDetailedView>> ListOrdersAsync(
            [FromServices] IEnforcementOrderRepository order,
            [FromQuery] EnforcementOrderSpec spec,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25) =>
            order.ListDetailedAsync(spec, new PaginationSpec(page, pageSize));

        [HttpGet(nameof(EnforcementOrder) + "/{id:int}")]
        public async Task<ActionResult<EnforcementOrderDetailedView>> GetOrderAsync(
            [FromServices] IEnforcementOrderRepository order,
            [FromRoute] int id)
        {
            var item = await order.GetAsync(id);
            return item != null ? Ok(item) : Problem("ID not found.", statusCode: 404);
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
}