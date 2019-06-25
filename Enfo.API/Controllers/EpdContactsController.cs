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
    public class EpdContactsController : ControllerBase
    {
        private readonly IAsyncWritableRepository<EpdContact> repository;

        public EpdContactsController(IAsyncWritableRepository<EpdContact> repository)
            => this.repository = repository;

        // GET: api/EpdContacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EpdContactResource>>> Get()
            => Ok((await repository.ListAsync().ConfigureAwait(false))
                .Select(e => new EpdContactResource(e)));

        // GET: api/EpdContacts/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EpdContactResource>> Get(int id)
        {
            var item = await repository.GetByIdAsync(id, e => e.Address).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return new EpdContactResource(item);
        }

        // POST: api/EpdContacts
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(EpdContactResource resource)
        {
            var item = resource.NewEpdContact();
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/EpdContacts/5
        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EpdContactResource resource)
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
