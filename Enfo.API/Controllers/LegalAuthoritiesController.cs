using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalAuthoritiesController : ControllerBase
    {
        private readonly IAsyncWritableRepository<LegalAuthority> repository;

        public LegalAuthoritiesController(IAsyncWritableRepository<LegalAuthority> repository)
            => this.repository = repository;

        // GET: api/LegalAuthorities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegalAuthorityResource>>> Get()
            => Ok((await repository.ListAsync().ConfigureAwait(false))
                .Select(e => new LegalAuthorityResource(e)));

        // GET: api/LegalAuthorities/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LegalAuthorityResource>> Get(int id)
        {
            var item = await repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return new LegalAuthorityResource(item);
        }

        // POST: api/LegalAuthorities
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(
            LegalAuthorityResource resource)
        {
            var item = resource.NewLegalAuthority();
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/LegalAuthorities/5
        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            LegalAuthorityResource resource)
        {
            if (id != resource.Id)
            {
                return BadRequest();
            }

            var item = await repository.GetByIdAsync(id).ConfigureAwait(false);
            item.UpdateFrom(resource);
            await repository.CompleteAsync().ConfigureAwait(false);

            return Ok(resource);
        }
    }
}
