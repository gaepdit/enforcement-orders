using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.API.QueryStrings;
using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using Enfo.Domain.Repositories;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpdContactsController : ControllerBase
    {
        private readonly IAsyncWritableRepository<EpdContact> _repository;

        public EpdContactsController(IAsyncWritableRepository<EpdContact> repository) =>
            _repository = repository;

        // GET: api/EpdContacts?pageSize&page
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EpdContactResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EpdContactResource>>> Get(
            [FromQuery] ActiveItemFilter filter = null,
            [FromQuery] PaginationFilter paging = null)
        {
            filter ??= new ActiveItemFilter();
            paging ??= new PaginationFilter();

            var spec = new FilterByActiveItems<EpdContact>(filter.IncludeInactive);
            var pagination = paging.Pagination();
            var include = new EpdContactIncludingAddress();

            return Ok((await _repository.ListAsync(spec, pagination, inclusion: include)
                .ConfigureAwait(false))
                .Select(e => new EpdContactResource(e)));
        }

        // GET: api/EpdContacts/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(EpdContactResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EpdContactResource>> Get(int id)
        {
            var item = await _repository.GetByIdAsync(id,
                inclusion: new EpdContactIncludingAddress()
            ).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            return Ok(new EpdContactResource(item));
        }

        // POST: api/EpdContacts
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] EpdContactCreateResource resource)
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
            int id,
            [FromBody] EpdContactUpdateResource resource)
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
