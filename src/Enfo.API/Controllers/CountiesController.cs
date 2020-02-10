using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.API.QueryStrings;
using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountiesController : ControllerBase
    {
        private readonly IAsyncReadableRepository<County> _repository;

        public CountiesController(IAsyncReadableRepository<County> repository) =>
            _repository = repository;

        // GET: api/Counties?pageSize&page
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CountyResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CountyResource>>> Get(
            [FromQuery] PaginationFilter paging = null)
        {
            paging ??= new PaginationFilter();
            return Ok((await _repository.ListAsync(pagination: paging.Pagination())
                .ConfigureAwait(false))
                .Select(e => new CountyResource(e)));
        }

        // GET: api/Counties/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CountyResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountyResource>> Get(
            [FromRoute] int id)
        {
            var item = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            return Ok(new CountyResource(item));
        }
    }
}
