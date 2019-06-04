using Enfo.Domain.Resources;
using Enfo.Domain.Services;
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

        //// PUT: api/LegalAuthorities/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutLegalAuthority(int id, LegalAuthorityResource legalAuthority)
        //{
        //    if (id != legalAuthority.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(legalAuthority).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LegalAuthorityExistsAsync(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/LegalAuthorities
        //[HttpPost]
        //public async Task<ActionResult<LegalAuthority>> PostLegalAuthority(LegalAuthority legalAuthority)
        //{
        //    _context.LegalAuthorities.Add(legalAuthority);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetLegalAuthority", new { id = legalAuthority.Id }, legalAuthority);
        //}

        private async Task<bool> LegalAuthorityExistsAsync(int id) 
            => await service.ExistsAsync(id).ConfigureAwait(false);
    }
}
