using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using Enfo.Domain.Repositories;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enfo.API.ApiPagination;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalAuthoritiesController : ControllerBase
    {
        private readonly IAsyncWritableRepository<LegalAuthority> _repository;

        public LegalAuthoritiesController(IAsyncWritableRepository<LegalAuthority> repository) =>
            _repository = repository;

        // GET: api/LegalAuthorities?pageSize&page
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LegalAuthorityResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<LegalAuthorityResource>>> Get(
            int pageSize = DefaultPageSize,
            int page = 1,
            bool includeInactive = false)
        {
            var spec = new ActiveItemsSpec<LegalAuthority>(includeInactive);

            return Ok((await _repository.ListAsync(spec, Paginate(pageSize, page))
                .ConfigureAwait(false))
                .Select(e => new LegalAuthorityResource(e)));
        }

        // GET: api/LegalAuthorities/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LegalAuthorityResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LegalAuthorityResource>> Get(int id)
        {
            var item = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(new LegalAuthorityResource(item));
        }

        // POST: api/LegalAuthorities
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] LegalAuthorityCreateResource resource)
        {
            if (resource is null)
            {
                return BadRequest();
            }

            var item = resource.NewLegalAuthority;
            _repository.Add(item);
            await _repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/LegalAuthorities/5
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LegalAuthorityResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] LegalAuthorityResource resource)
        {
            if (resource is null || id != resource.Id)
            {
                return BadRequest();
            }

            var item = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            item.UpdateFrom(resource);
            await _repository.CompleteAsync().ConfigureAwait(false);

            return Ok(resource);
        }
    }
}
