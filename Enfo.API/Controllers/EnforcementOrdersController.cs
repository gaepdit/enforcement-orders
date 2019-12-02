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
using static Enfo.API.ApiPagination;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnforcementOrdersController : ControllerBase
    {
        private readonly IAsyncWritableRepository<EnforcementOrder> repository;

        public EnforcementOrdersController(IAsyncWritableRepository<EnforcementOrder> repository)
            => this.repository = repository;

        // GET: api/EnforcementOrders?params
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderResource>>> Get(
            string facilityFilter = null,
            string county = null,
            int? legalAuth = null,
            DateTime? fromDate = null,
            DateTime? tillDate = null,
            ActivityStatus status = ActivityStatus.All,
            PublicationStatus publicationStatus = PublicationStatus.Published,
            string orderNumber = null,
            string textContains = null,
            EnforcementOrderSorting sortOrder = EnforcementOrderSorting.FacilityAsc,
            int pageSize = DefaultPageSize,
            int page = 1)
        {
            // TODO: Only authorized users can request Orders with PublicationStatus other than "Published".
            //if (!User.LoggedIn)
            //{
            //    publicationStatus = PublicationStatus.Published;
            //}

            // Specifications
            ISpecification<EnforcementOrder> spec = new TrueSpec<EnforcementOrder>();
            
            // TODO: Only authorized users can request Orders that are not public.
            //if (!User.LoggedIn)
            //{
            //    spec = spec.And(new PublicOrdersSpec());
            //}
            if (!facilityFilter.IsNullOrWhiteSpace())
            {
                spec = spec.And(new FilterOrdersByName(facilityFilter));
            }
            if (!county.IsNullOrWhiteSpace())
            {
                spec = spec.And(new FilterOrdersByCounty(county));
            }
            if (legalAuth.HasValue)
            {
                spec = spec.And(new FilterOrdersByLegalAuth(legalAuth.Value));
            }
            if (fromDate.HasValue)
            {
                spec = spec.And(new FilterOrdersByStartDate(fromDate.Value, status));
            }
            if (tillDate.HasValue)
            {
                spec = spec.And(new FilterOrdersByEndDate(tillDate.Value, status));
            }
            if (status != ActivityStatus.All)
            {
                spec = spec.And(new FilterOrdersByActivityStatus(status));
            }
            if (publicationStatus != PublicationStatus.All)
            {
                spec = spec.And(new FilterOrdersByPublicationStatus(publicationStatus));
            }
            if (!orderNumber.IsNullOrWhiteSpace())
            {
                spec = spec.And(new FilterOrdersByOrderNumber(orderNumber));
            }
            if (!textContains.IsNullOrWhiteSpace())
            {
                spec = spec.And(new FilterOrdersByText(textContains));
            }

            // Paging
            var paging = Paginate(pageSize, page);

            // Sorting
            // BUG: Sorting by date currently broken
            var sorting = new SortEnforcementOrders(sortOrder);

            return Ok((await repository.ListAsync(spec, paging, sorting)
                .ConfigureAwait(false))
                .Select(e => new EnforcementOrderResource(e)));
        }

        // GET: api/EnforcementOrders/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EnforcementOrderResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnforcementOrderResource>> Get(int id)
        {
            // Ensure specification is set to exclude non-public data
            ISpecification<EnforcementOrder> spec = new PublicOrdersSpec();

            var item = await repository.GetByIdAsync(id, spec).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return new EnforcementOrderResource(item);
        }

        // GET: api/EnforcementOrders/Details/5
        //[Authorize]
        [HttpGet("Details/{id}")]
        [ProducesResponseType(typeof(EnforcementOrderDetailedResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnforcementOrderDetailedResource>> Details(int id)
        {
            var item = await repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return new EnforcementOrderDetailedResource(item);
        }

        // GET: api/EnforcementOrders?params
        [HttpGet("Count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> Count(
            string facilityFilter = "",
            string county = "",
            int? legalAuth = null,
            DateTime? fromDate = null,
            DateTime? tillDate = null,
            ActivityStatus status = ActivityStatus.All,
            PublicationStatus publicationStatus = PublicationStatus.Published,
            string orderNumber = "",
            string textContains = "")
        {
            throw new NotImplementedException();

            // TODO: Only authorized users can request Orders with PublicationStatus other than "Published".
            //if (!User.LoggedIn)
            //{
            //    publicationStatus = PublicationStatus.Published;
            //}
        }

        // GET: api/EnforcementOrders/CurrentProposed
        [HttpGet("CurrentProposed")]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderResource>>> CurrentProposed(
            int pageSize = DefaultPageSize,
            int page = 1)
        {
            // Current Proposed are public proposed orders with comment close date in the future and publication date in the past
            throw new NotImplementedException();
        }

        // GET: api/EnforcementOrders/RecentlyExecuted
        [HttpGet("RecentlyExecuted")]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderResource>>> RecentlyExecuted(
            int pageSize = DefaultPageSize,
            int page = 1)
        {
            // Recently Executed are public executed orders with publication date within current week
            throw new NotImplementedException();
        }

        // GET: api/EnforcementOrders/Draft
        //[Authorize]
        [HttpGet("Draft")]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderResource>>> Drafts(
            int pageSize = DefaultPageSize,
            int page = 1)
        {
            // Draft are orders with publication status set to Draft
            throw new NotImplementedException();
        }

        // GET: api/EnforcementOrders/Pending
        //[Authorize]
        [HttpGet("Pending")]
        [ProducesResponseType(typeof(IEnumerable<EnforcementOrderResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderResource>>> Pending(
            int pageSize = DefaultPageSize,
            int page = 1)
        {
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
