using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalAuthoritiesController : ControllerBase
    {
        private readonly ILegalAuthorityRepository repository;

        public LegalAuthoritiesController(ILegalAuthorityRepository repository)
            => this.repository = repository;

        // GET: api/LegalAuthorities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegalAuthorityResource>>> GetAllAsync()
            => Ok((await repository.ListAllAsync().ConfigureAwait(false))
                .Select(e => new LegalAuthorityResource(e)));

        // GET: api/LegalAuthorities/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LegalAuthorityResource>> GetByIdAsync(int id)
        {
            var legalAuthority = await repository.GetByIdAsync(id)
                .ConfigureAwait(false);

            if (legalAuthority == null)
            {
                return NotFound();
            }

            return new LegalAuthorityResource(legalAuthority);
        }

        // PUT: api/LegalAuthorities/5
        // [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLegalAuthority(
            int id,
            LegalAuthorityResource value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            var item = await repository.GetByIdAsync(id).ConfigureAwait(false);

            item.Active = value.Active;
            item.AuthorityName = value.AuthorityName;
            item.OrderNumberTemplate = value.OrderNumberTemplate;
            item.UpdatedDate = DateTime.Now;

            await repository.CompleteAsync().ConfigureAwait(false);

            return Ok(value);
        }

        // POST: api/LegalAuthorities
        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostLegalAuthority(
            LegalAuthorityResource resource)
        {
            var item = new LegalAuthority()
            {
                AuthorityName = resource.AuthorityName,
                OrderNumberTemplate = resource.OrderNumberTemplate,
                Active = resource.Active
            };

            repository.Add(item);

            await repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(GetByIdAsync), item.Id);
        }
    }
}
