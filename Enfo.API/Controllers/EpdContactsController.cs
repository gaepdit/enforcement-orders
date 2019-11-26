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
    public class EpdContactsController : ControllerBase
    {
        private readonly IAsyncWritableRepository<EpdContact> repository;

        public EpdContactsController(IAsyncWritableRepository<EpdContact> repository) =>
            this.repository = repository;

        // GET: api/EpdContacts?pageSize&page
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EpdContactResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EpdContactResource>>> Get(
            int pageSize = DefaultPageSize,
            int page = 1,
            bool includeInactive = false)
        {
            var spec = new ActiveItemsSpec<EpdContact>(includeInactive);
            var paging = Paginate(pageSize, page);
            var include = new EpdContactIncludingAddress();

            return Ok((await repository.ListAsync(spec, paging, inclusion: include)
                .ConfigureAwait(false))
                .Select(e => new EpdContactResource(e)));
        }

        // GET: api/EpdContacts/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(EpdContactResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EpdContactResource>> Get(int id)
        {
            var item = await repository.GetByIdAsync(id,
                new ActiveItemsSpec<EpdContact>(true),
                new EpdContactIncludingAddress()
            ).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return new EpdContactResource(item);
        }

        // POST: api/EpdContacts
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] EpdContactCreateResource resource)
        {
            var item = resource.NewEpdContact();
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/EpdContacts/5
        //[Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EpdContactUpdateResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] EpdContactUpdateResource resource)
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
