using System.Linq;
using System.Threading.Tasks;
using Enfo.API.Classes;
using Enfo.API.QueryStrings;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.County;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountiesController : ControllerBase
    {
        private readonly IReadOnlyRepository<County> _repository;

        public CountiesController(IReadOnlyRepository<County> repository) =>
            _repository = repository;

        // GET: api/Counties?pageSize&page
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaginatedList<CountyView>>> Get(
            [FromQuery] PaginationFilter paging = null)
        {
            paging ??= new PaginationFilter();

            var countTask = _repository.CountAsync().ConfigureAwait(false);
            var itemsTask = _repository.ListAsync(pagination: paging.Pagination()).ConfigureAwait(false);

            var paginatedList = (await itemsTask)
                .Select(e => new CountyView(e))
                .GetPaginatedList(await countTask, paging);

            return Ok(paginatedList);
        }

        // GET: api/Counties/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountyView>> Get(
            [FromRoute] int id)
        {
            var item = await _repository.GetAsync(id).ConfigureAwait(false);

            if (item is null)
            {
                return NotFound(id);
            }

            return Ok(new CountyView(item));
        }
    }
}
