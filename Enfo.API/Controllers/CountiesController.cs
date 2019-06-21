using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountiesController : ControllerBase
    {
        private readonly IAsyncReadableRepository<County> repository;

        public CountiesController(IAsyncReadableRepository<County> repository)
            => this.repository = repository;

        // GET: api/Counties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountyResource>>> Get()
            => Ok((await repository.ListAllAsync().ConfigureAwait(false))
                .Select(e => new CountyResource(e)));

        // GET: api/Counties/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountyResource>> Get(int id)
        {
            var item = await repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return new CountyResource(item);
        }
    }
}
