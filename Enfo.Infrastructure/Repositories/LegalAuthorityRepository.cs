using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.Repositories
{
    public class LegalAuthorityRepository : BaseRepository<LegalAuthority>, ILegalAuthorityRepository
    {
        public LegalAuthorityRepository(EnfoDbContext context) : base(context) { }
    }
}