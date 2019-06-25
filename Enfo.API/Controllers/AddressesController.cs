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
    public class AddressesController : ControllerBase
    {
        private readonly IAsyncWritableRepository<Address> repository;

        public AddressesController(IAsyncWritableRepository<Address> repository)
            => this.repository = repository;

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressResource>>> Get()
            => Ok((await repository.ListAsync().ConfigureAwait(false))
                .Select(e => new AddressResource(e)));

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressResource>> Get(int id)
        {
            var item = await repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return new AddressResource(item);
        }

        // POST: api/Addresses
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(
            AddressResource resource)
        {
            var item = resource.NewAddress();
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), item.Id);
        }

        // PUT: api/Addresses/5
        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            AddressResource resource)
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
