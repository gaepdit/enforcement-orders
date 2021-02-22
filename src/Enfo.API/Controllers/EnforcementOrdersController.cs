using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.API.Classes;
using Enfo.API.QueryStrings;
using Enfo.Repository.Resources;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnforcementOrdersController : ControllerBase
    {
        private readonly IEnforcementOrderRepository _repository;

        public EnforcementOrdersController(IEnforcementOrderRepository repository) =>
            _repository = repository;

        // GET: api/EnforcementOrders?params
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaginatedList<EnforcementOrderSummaryView>>> Get(
            [FromQuery] EnforcementOrderFilter filter = null,
            [FromQuery] PaginationFilter paging = null)
        {
            // Specifications
            filter ??= new EnforcementOrderFilter();

            bool onlyIfPublic = false;
            // TODO: Only authorized users can request Orders that are not public.
            // bool onlyIfPublic = !User.LoggedIn;

            // TODO: Only authorized users can request Orders with PublicationStatus other than "Published"
            // or deleted Orders
            //if (!User.LoggedIn)
            //{
            //    filter.PublicationStatus = PublicationStatus.Published;
            //    filter.IncludeDeleted = false;
            //}

            // Paging
            var pagination = (paging ??= new PaginationFilter()).Pagination();

            var countTask = _repository.CountEnforcementOrdersAsync(
                filter.FacilityFilter, filter.County, filter.LegalAuth, filter.FromDate, filter.TillDate, filter.Status,
                filter.PublicationStatus, filter.OrderNumber, filter.TextContains, onlyIfPublic, filter.Deleted)
                .ConfigureAwait(false);

            var itemsTask = _repository.FindEnforcementOrdersAsync(
                filter.FacilityFilter, filter.County, filter.LegalAuth, filter.FromDate, filter.TillDate, filter.Status,
                filter.PublicationStatus, filter.OrderNumber, filter.TextContains, onlyIfPublic, filter.Deleted,
                filter.SortOrder, pagination)
                .ConfigureAwait(false);

            var paginatedList = (await itemsTask)
                .Select(e => new EnforcementOrderSummaryView(e))
                .GetPaginatedList(await countTask, paging);

            return Ok(paginatedList);
        }

        // GET: api/EnforcementOrders/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EnforcementOrderView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnforcementOrderView>> Get(
            [FromRoute] int id)
        {
            bool onlyIfPublic = false;
            // TODO: Only authorized users can request Orders that are not public.
            // bool onlyIfPublic = !User.LoggedIn;

            var item = await _repository
                .GetEnforcementOrder(id, onlyIfPublic)
                .ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            return Ok(new EnforcementOrderView(item));
        }

        // GET: api/EnforcementOrders/Details/5
        //[Authorize]
        [HttpGet("Details/{id}")]
        [ProducesResponseType(typeof(EnforcementOrderDetailedView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnforcementOrderDetailedView>> Details(
            [FromRoute] int id)
        {
            bool onlyIfPublic = false;
            // TODO: Only authorized users can request Orders that are not public.
            // bool onlyIfPublic = !User.LoggedIn;

            var item = await _repository
                .GetEnforcementOrder(id, onlyIfPublic)
                .ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            return Ok(new EnforcementOrderDetailedView(item));
        }

        // GET: api/EnforcementOrders?params
        [HttpGet("Count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> Count(
            [FromQuery] EnforcementOrderFilter filter = null)
        {
            // Specifications
            filter ??= new EnforcementOrderFilter();

            bool onlyIfPublic = false;
            // TODO: Only authorized users can request Orders that are not public.
            // bool onlyIfPublic = !User.LoggedIn;

            // TODO: Only authorized users can request Orders with PublicationStatus other than "Published"
            // or deleted Orders
            //if (!User.LoggedIn)
            //{
            //    filter.PublicationStatus = PublicationStatus.Published;
            //    filter.IncludeDeleted = false;
            //}

            return Ok(await _repository
                .CountEnforcementOrdersAsync(filter.FacilityFilter, filter.County,
                    filter.LegalAuth, filter.FromDate, filter.TillDate, filter.Status,
                    filter.PublicationStatus, filter.OrderNumber, filter.TextContains,
                    onlyIfPublic, filter.Deleted)
                .ConfigureAwait(false));
        }

        // GET: api/EnforcementOrders/CurrentProposed
        [HttpGet("CurrentProposed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderSummaryView>>> CurrentProposed()
        {
            return Ok((await _repository
                .FindCurrentProposedEnforcementOrders()
                .ConfigureAwait(false))
                .Select(e => new EnforcementOrderSummaryView(e)));
        }

        // GET: api/EnforcementOrders/RecentlyExecuted
        [HttpGet("RecentlyExecuted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderSummaryView>>> RecentlyExecuted()
        {
            return Ok((await _repository
                .FindRecentlyExecutedEnforcementOrders()
                .ConfigureAwait(false))
                .Select(e => new EnforcementOrderSummaryView(e)));
        }

        // GET: api/EnforcementOrders/Draft
        //[Authorize]
        [HttpGet("Draft")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderSummaryView>>> Drafts()
        {
            return Ok((await _repository
                .FindDraftEnforcementOrders()
                .ConfigureAwait(false))
                .Select(e => new EnforcementOrderSummaryView(e)));
        }

        // GET: api/EnforcementOrders/Pending
        //[Authorize]
        [HttpGet("Pending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnforcementOrderSummaryView>>> Pending()
        {
            return Ok((await _repository
                .FindPendingEnforcementOrders()
                .ConfigureAwait(false))
                .Select(e => new EnforcementOrderSummaryView(e)));
        }

        // POST: api/EnforcementOrders
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(
            [FromBody] EnforcementOrderCreate resource)
        {
            if (resource is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository
                .CreateEnforcementOrderAsync(
                    resource.CreateAs, resource.Cause, resource.CommentContactId, resource.CommentPeriodClosesDate,
                    resource.County, resource.FacilityName, resource.ExecutedDate, resource.ExecutedOrderPostedDate,
                    resource.HearingCommentPeriodClosesDate, resource.HearingContactId, resource.HearingDate,
                    resource.HearingLocation, resource.IsHearingScheduled, resource.LegalAuthorityId, resource.OrderNumber,
                    resource.ProposedOrderPostedDate, resource.PublicationStatus, resource.Requirements,
                    resource.SettlementAmount)
                .ConfigureAwait(false);

            if (result.Success)
            {
                return CreatedAtAction(nameof(Get), result.NewItem.Id);
            }

            foreach (var message in result.ErrorMessages)
            {
                ModelState.TryAddModelError(message.Key, message.Value);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/EnforcementOrders/5
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromRoute] int id,
            [FromBody] EnforcementOrderUpdate resource)
        {
            if (resource is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemExists = await _repository.IdExists(id).ConfigureAwait(false);

            if (!itemExists)
            {
                return NotFound(id);
            }

            var result = await _repository.UpdateEnforcementOrderAsync(
                id, resource.Cause, resource.CommentContactId, resource.CommentPeriodClosesDate,
                resource.County, resource.FacilityName, resource.ExecutedDate, resource.ExecutedOrderPostedDate,
                resource.HearingCommentPeriodClosesDate, resource.HearingContactId, resource.HearingDate,
                resource.HearingLocation, resource.IsExecutedOrder, resource.IsHearingScheduled,
                resource.LegalAuthorityId, resource.OrderNumber, resource.ProposedOrderPostedDate,
                resource.PublicationStatus, resource.Requirements, resource.SettlementAmount)
                .ConfigureAwait(false);

            if (result.Success)
            {
                return NoContent();
            }

            foreach (var message in result.ErrorMessages)
            {
                ModelState.TryAddModelError(message.Key, message.Value);
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/EnforcementOrders/5
        // [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            if (item.Deleted)
            {
                ModelState.AddModelError(nameof(id), "Enforcement order is already deleted.");
                return BadRequest(ModelState);
            }

            item.Deleted = true;
            await _repository.CompleteAsync().ConfigureAwait(false);

            return NoContent();
        }

        // PUT: api/EnforcementOrders/Undelete/5
        // [Authorize]
        [HttpPut("Undelete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Undelete(
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            if (!item.Deleted)
            {
                ModelState.AddModelError(nameof(id), "Enforcement order is not deleted.");
                return BadRequest(ModelState);
            }

            item.Deleted = false;
            await _repository.CompleteAsync().ConfigureAwait(false);

            return NoContent();
        }
    }
}
