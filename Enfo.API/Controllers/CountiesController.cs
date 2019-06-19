using Enfo.API.Resources;
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
        private readonly ICountyRepository repository;

        public CountiesController(ICountyRepository repository)
            => this.repository = repository;

        // GET: api/Counties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountyResource>>> GetAllAsync()
            => Ok((await repository.ListAllAsync().ConfigureAwait(false))
                .Select(e => new CountyResource(e)));

        // GET: api/Counties/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountyResource>> GetByIdAsync(int id)
        {
            var county = await repository.GetByIdAsync(id)
                .ConfigureAwait(false);

            if (county == null)
            {
                return NotFound();
            }

            return new CountyResource(county);
        }
    }
}
