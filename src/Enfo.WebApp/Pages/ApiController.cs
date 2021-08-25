using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.Domain.Specs;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.WebApp.Pages.Api
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        private readonly IEnforcementOrderRepository _order;
        private readonly ILegalAuthorityRepository _legalAuthority;

        public ApiController(
            IEnforcementOrderRepository order,
            ILegalAuthorityRepository legalAuthority) =>
            (_order, _legalAuthority) = (order, legalAuthority);

        [HttpGet(nameof(EnforcementOrder))]
        public Task<PaginatedResult<EnforcementOrderDetailedView>> ListOrdersAsync(
            [FromQuery] EnforcementOrderSpec spec,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25) =>
            _order.ListDetailedAsync(spec, new PaginationSpec(page, pageSize));

        [HttpGet(nameof(EnforcementOrder) + "/{id:int}")]
        public Task<EnforcementOrderDetailedView> GetOrderAsync(
            [FromRoute] int id) =>
            _order.GetAsync(id);

        [HttpGet(nameof(LegalAuthority))]
        public Task<IReadOnlyList<LegalAuthorityView>> ListLegalAuthoritiesAsync(
            [FromQuery] bool includeInactive = false) =>
            _legalAuthority.ListAsync(includeInactive);

        [HttpGet(nameof(LegalAuthority) + "/{id:int}")]
        public Task<LegalAuthorityView> GetLegalAuthorityAsync(
            [FromRoute] int id) =>
            _legalAuthority.GetAsync(id);
    }
}