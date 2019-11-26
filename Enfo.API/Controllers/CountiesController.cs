using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
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
    public class CountiesController : ControllerBase
    {
        private readonly IAsyncReadableRepository<County> repository;

        public CountiesController(IAsyncReadableRepository<County> repository) =>
            this.repository = repository;

        // GET: api/Counties?pageSize&page
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CountyResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CountyResource>>> Get(
            int pageSize = DefaultPageSize,
            int page = 1)
        {
            return Ok((await repository.ListAsync(pagination: Paginate(pageSize, page))
                .ConfigureAwait(false))
                .Select(e => new CountyResource(e)));
        }

        // GET: api/Counties/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CountyResource), StatusCodes.Status200OK)]
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
