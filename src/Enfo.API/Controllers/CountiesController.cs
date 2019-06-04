using Enfo.Domain.Resources;
using Enfo.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountiesController : ControllerBase
    {
        private readonly ICountyService service;

        public CountiesController(ICountyService countyService) 
            => service = countyService;

        // GET: api/Counties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountyResource>>> GetAllAsync()
            => Ok(await service.GetAllAsync().ConfigureAwait(false));

        // GET: api/Counties/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountyResource>> GetByIdAsync(int id)
        {
            var county = await service.GetByIdAsync(id)
                .ConfigureAwait(false);

            if (county == null)
            {
                return NotFound();
            }

            return county;
        }

        // GET: api/Counties/name/{name}
        [HttpGet("name/{name}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountyResource>> GetByNameAsync(string name)
        {
            var county = await service.GetByNameAsync(name)
                .ConfigureAwait(false);

            if (county == null)
            {
                return NotFound();
            }

            return county;
        }
    }
}
