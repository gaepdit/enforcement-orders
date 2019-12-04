using Enfo.API.QueryStrings;
using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using Enfo.Domain.Repositories;
using Enfo.Domain.Utils;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnforcementOrdersController : ControllerBase
    {
        private readonly IAsyncWritableRepository<EnforcementOrder> _repository;

        public EnforcementOrdersController(IAsyncWritableRepository<EnforcementOrder> repository) =>
            _repository = repository;

        // GET: api/EnforcementOrders?params
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderListResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderListResource>>> Get(
            [FromQuery] EnforcementOrderFilter filter = null,
            [FromQuery] PaginationFilter paging = null)
        {
            filter ??= new EnforcementOrderFilter();
            paging ??= new PaginationFilter();

            // TODO: Only authorized users can request Orders with PublicationStatus other than "Published".
            //if (!User.LoggedIn)
            //    publicationStatus = PublicationStatus.Published;

            // Specifications
            ISpecification<EnforcementOrder> spec = new TrueSpec<EnforcementOrder>();

            // TODO: Only authorized users can request Orders that are not public.
            //if (!User.LoggedIn)
            //    spec = spec.And(new PublicOrdersSpec());

            if (!filter.FacilityFilter.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByName(filter.FacilityFilter));

            if (!filter.County.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByCounty(filter.County));

            if (filter.LegalAuth.HasValue)
                spec = spec.And(new FilterOrdersByLegalAuth(filter.LegalAuth.Value));

            if (filter.FromDate.HasValue)
                spec = spec.And(new FilterOrdersByStartDate(filter.FromDate.Value, filter.Status));

            if (filter.TillDate.HasValue)
                spec = spec.And(new FilterOrdersByEndDate(filter.TillDate.Value, filter.Status));

            if (filter.Status != ActivityStatus.All)
                spec = spec.And(new FilterOrdersByActivityStatus(filter.Status));

            if (filter.PublicationStatus != PublicationStatus.All)
                spec = spec.And(new FilterOrdersByPublicationStatus(filter.PublicationStatus));

            if (!filter.OrderNumber.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByOrderNumber(filter.OrderNumber));

            if (!filter.TextContains.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByText(filter.TextContains));

            // Paging
            var pagination = paging.Pagination();

            // Sorting
            // BUG: Sorting by date currently broken
            var sorting = new SortEnforcementOrders(filter.SortOrder);

            // Including
            var include = new EnforcementOrderIncludeLegalAuth();

            return Ok((await _repository
                .ListAsync(spec, pagination, sorting, include)
                .ConfigureAwait(false))
                .Select(e => new EnforcementOrderListResource(e)));
        }

        // GET: api/EnforcementOrders/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EnforcementOrderItemResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnforcementOrderItemResource>> Get(int id)
        {
            ISpecification<EnforcementOrder> spec = new TrueSpec<EnforcementOrder>();

            // TODO: Only authorized users can request Orders that are not public.
            //if (!User.LoggedIn)
            //{
            //    spec = spec.And(new PublicOrdersSpec());
            //}
            // Ensure specification is set to exclude non-public data

            var include = new EnforcementOrderIncludeAll();

            var item = await _repository
                .GetByIdAsync(id, spec, include)
                .ConfigureAwait(false);

            if (item == null) return NotFound();

            return Ok(new EnforcementOrderItemResource(item));
        }

        // GET: api/EnforcementOrders/Details/5
        //[Authorize]
        [HttpGet("Details/{id}")]
        [ProducesResponseType(typeof(EnforcementOrderDetailedResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnforcementOrderDetailedResource>> Details(int id)
        {
            var include = new EnforcementOrderIncludeAll();

            var item = await _repository
                .GetByIdAsync(id, inclusion: include)
                .ConfigureAwait(false);

            if (item == null) return NotFound();

            return Ok(new EnforcementOrderDetailedResource(item));
        }

        // GET: api/EnforcementOrders?params
        [HttpGet("Count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> Count(
            [FromQuery] EnforcementOrderFilter filter = null)
        {
            filter ??= new EnforcementOrderFilter();

            throw new NotImplementedException();

            // TODO: Only authorized users can request Orders with PublicationStatus other than "Published".
            //if (!User.LoggedIn)
            //{
            //    publicationStatus = PublicationStatus.Published;
            //}
        }

        // GET: api/EnforcementOrders/CurrentProposed
        [HttpGet("CurrentProposed")]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderListResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderListResource>>> CurrentProposed(
            [FromQuery] PaginationFilter filter = null)
        {
            filter ??= new PaginationFilter();

            // Current Proposed are public proposed orders with comment close date in the future and publication date in the past
            throw new NotImplementedException();
        }

        // GET: api/EnforcementOrders/RecentlyExecuted
        [HttpGet("RecentlyExecuted")]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderListResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderListResource>>> RecentlyExecuted(
            [FromQuery] PaginationFilter filter = null)
        {
            filter ??= new PaginationFilter();

            // Recently Executed are public executed orders with publication date within current week
            throw new NotImplementedException();
        }

        // GET: api/EnforcementOrders/Draft
        //[Authorize]
        [HttpGet("Draft")]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderListResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderListResource>>> Drafts(
            [FromQuery] PaginationFilter filter = null)
        {
            filter ??= new PaginationFilter();

            // Draft are orders with publication status set to Draft
            throw new NotImplementedException();
        }

        // GET: api/EnforcementOrders/Pending
        //[Authorize]
        [HttpGet("Pending")]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderListResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderListResource>>> Pending(
            [FromQuery] PaginationFilter filter = null)
        {
            filter ??= new PaginationFilter();

            // Pending are public proposed or executed orders with publication date after the current week
            throw new NotImplementedException();
        }

        //// POST: api/EnforcementOrders
        ////[Authorize]
        //[HttpPost]
        //[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Post(EnforcementOrderCreateResource resource)
        //{
        //    throw new NotImplementedException();
        //}

        //// PUT: api/Orders/5
        ////[Authorize]
        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(EnforcementOrderUpdateResource), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Put(int id, EnforcementOrderUpdateResource resource)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
