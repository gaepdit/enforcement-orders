using Enfo.Models.Resources;
using Enfo.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalAuthoritiesController : ControllerBase
    {
        private readonly ILegalAuthorityService service;

        public LegalAuthoritiesController(ILegalAuthorityService legalAuthorityService)
            => service = legalAuthorityService;

        // GET: api/LegalAuthorities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegalAuthorityResource>>> GetAllAsync()
            => Ok(await service.GetAllAsync().ConfigureAwait(false));

        // GET: api/LegalAuthorities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LegalAuthorityResource>> GetByIdAsync(int id)
        {
            var legalAuthority = await service.GetByIdAsync(id)
                .ConfigureAwait(false);

            if (legalAuthority == null)
            {
                return NotFound();
            }

            return legalAuthority;
        }
    }
}
