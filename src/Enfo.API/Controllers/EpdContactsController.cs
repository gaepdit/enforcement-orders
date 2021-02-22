using System.Linq;
using System.Threading.Tasks;
using Enfo.API.Classes;
using Enfo.API.QueryStrings;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources;
using Enfo.Repository.Querying;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EpdContact;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpdContactsController : ControllerBase
    {
        private readonly IWritableRepository<EpdContact> _repository;

        public EpdContactsController(IWritableRepository<EpdContact> repository) =>
            _repository = repository;

        // GET: api/EpdContacts?pageSize&page
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaginatedList<EpdContactView>>> Get(
            [FromQuery] ActiveItemFilter filter = null,
            [FromQuery] PaginationFilter paging = null)
        {
            filter ??= new ActiveItemFilter();
            paging ??= new PaginationFilter();

            var spec = new FilterByActiveItems<EpdContact>(filter.IncludeInactive);
            var pagination = paging.Pagination();
            var include = new EpdContactIncludingAddress();

            var countTask = _repository.CountAsync(spec).ConfigureAwait(false);
            var itemsTask = _repository.ListAsync(spec, pagination, inclusion: include).ConfigureAwait(false);

            var paginatedList = (await itemsTask)
                .Select(e => new EpdContactView(e))
                .GetPaginatedList(await countTask, paging);

            return Ok(paginatedList);
        }

        // GET: api/EpdContacts/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EpdContactView>> Get(
            [FromRoute] int id)
        {
            var item = await _repository.GetByIdAsync(id,
                inclusion: new EpdContactIncludingAddress()
            ).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            return Ok(new EpdContactView(item));
        }

        // POST: api/EpdContacts
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(
            [FromBody] EpdContactCreate resource)
        {
            if (resource is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = resource.NewEpdContact();
            _repository.Add(item);
            await _repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/EpdContacts/5
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromRoute] int id,
            [FromBody] EpdContactUpdate resource)
        {
            if (resource is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            item.UpdateFrom(resource);
            await _repository.CompleteAsync().ConfigureAwait(false);

            return NoContent();
        }
    }
}
