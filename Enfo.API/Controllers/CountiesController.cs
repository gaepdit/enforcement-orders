using Enfo.API.Resources;
using Enfo.Domain.Entities;
using Enfo.Domain.Pagination;
using Enfo.Domain.Repositories;
using Enfo.Domain.Specifications;
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

        public CountiesController(IAsyncReadableRepository<County> repository) =>
            this.repository = repository;

        // GET: api/Counties?pageSize&pageIndex
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CountyResource>>> Get(
            int pageSize = 0,
            int pageIndex = 0) =>
            Ok((await repository
                .ListAsync(Pagination.FromPageSizeAndIndex(pageSize, pageIndex))
                .ConfigureAwait(false))
                .Select(e => new CountyResource(e)));

        // GET: api/Counties/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
