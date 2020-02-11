using System.Linq;
using System.Threading.Tasks;
using Enfo.API.Classes;
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
    public class LegalAuthoritiesController : ControllerBase
    {
        private readonly IWritableRepository<LegalAuthority> _repository;

        public LegalAuthoritiesController(IWritableRepository<LegalAuthority> repository) =>
            _repository = repository;

        // GET: api/LegalAuthorities?pageSize&page
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaginatedList<LegalAuthorityResource>>> Get(
            [FromQuery] ActiveItemFilter filter = null,
            [FromQuery] PaginationFilter paging = null)
        {
            filter ??= new ActiveItemFilter();
            paging ??= new PaginationFilter();

            var spec = new FilterByActiveItems<LegalAuthority>(filter.IncludeInactive);

            var countTask = _repository.CountAsync(spec).ConfigureAwait(false);
            var itemsTask = _repository.ListAsync(spec, paging.Pagination()).ConfigureAwait(false);

            var paginatedList = (await itemsTask)
                .Select(e => new LegalAuthorityResource(e))
                .GetPaginatedList(await countTask, paging);

            return Ok(paginatedList);
        }

        // GET: api/LegalAuthorities/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LegalAuthorityResource>> Get(
            [FromRoute] int id)
        {
            var item = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            return Ok(new LegalAuthorityResource(item));
        }

        // POST: api/LegalAuthorities
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(
            [FromBody] LegalAuthorityCreateResource resource)
        {
            if (resource is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = resource.NewLegalAuthority();
            _repository.Add(item);
            await _repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/LegalAuthorities/5
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromRoute] int id,
            [FromBody] LegalAuthorityUpdateResource resource)
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
