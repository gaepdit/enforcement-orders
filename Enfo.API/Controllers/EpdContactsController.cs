using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Pagination;
using Enfo.Domain.Repositories;
using Enfo.Domain.Specifications;
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
    public class EpdContactsController : ControllerBase
    {
        private readonly IAsyncWritableRepository<EpdContact> repository;

        public EpdContactsController(IAsyncWritableRepository<EpdContact> repository) =>
            this.repository = repository;

        // GET: api/EpdContacts?pageSize&pageIndex
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EpdContactResource>>> Get(
            int pageSize = 0,
            int pageIndex = 0,
            bool includeInactive = false)
        {
            var pagination = Pagination.FromPageSizeAndIndex(pageSize, pageIndex);
            var spec = new EpdContactIncludeAddressSpec(includeInactive);

            return Ok((await repository.ListAsync(spec, pagination).ConfigureAwait(false))
                .Select(e => new EpdContactResource(e)));
        }

        // GET: api/EpdContacts/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EpdContactResource>> Get(int id)
        {
            var item = await repository.GetByIdAsync(id, new EpdContactIncludeAddressSpec(true)).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return new EpdContactResource(item);
        }

        // POST: api/EpdContacts
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(EpdContactCreateResource resource)
        {
            var item = resource.NewEpdContact();
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/EpdContacts/5
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, EpdContactUpdateResource resource)
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
