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
    public class AddressesController : ControllerBase
    {
        private readonly IAsyncWritableRepository<Address> _repository;

        public AddressesController(IAsyncWritableRepository<Address> repository) =>
            _repository = repository;

        // GET: api/Addresses?pageSize&page
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AddressResource>>> Get(
            [FromQuery] ActiveItemFilter filter = null,
            [FromQuery] PaginationFilter paging = null)
        {
            filter ??= new ActiveItemFilter();
            paging ??= new PaginationFilter();

            var spec = new FilterByActiveItems<Address>(filter.IncludeInactive);

            return Ok((await _repository.ListAsync(spec, paging.Pagination())
                .ConfigureAwait(false))
                .Select(e => new AddressResource(e)));
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressResource>> Get(int id)
        {
            var item = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            return Ok(new AddressResource(item));
        }

        // POST: api/Addresses
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AddressCreateResource resource)
        {
            if (resource is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = resource.NewAddress();
            _repository.Add(item);
            await _repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/Addresses/5
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] AddressUpdateResource resource)
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
