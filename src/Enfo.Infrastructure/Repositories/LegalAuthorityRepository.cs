using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Mapping;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.Domain.Utils;
using Enfo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Repositories
{
    public sealed class LegalAuthorityRepository : ILegalAuthorityRepository
    {
        private readonly EnfoDbContext _context;
        public LegalAuthorityRepository(EnfoDbContext context) => _context = Guard.NotNull(context, nameof(context));

        public async Task<LegalAuthorityView> GetAsync(int id)
        {
            var item = await _context.LegalAuthorities.AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

            return item == null ? null : new LegalAuthorityView(item);
        }

        public async Task<IReadOnlyList<LegalAuthorityView>> ListAsync(bool includeInactive = false) =>
            await _context.LegalAuthorities.AsNoTracking()
                .Where(e => e.Active || includeInactive)
                .OrderBy(e => e.AuthorityName).ThenBy(e => e.Id)
                .Select(e => new LegalAuthorityView(e))
                .ToListAsync().ConfigureAwait(false);


        public async Task<int> CreateAsync(LegalAuthorityCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));
            Guard.NotNullOrWhiteSpace(resource.AuthorityName, nameof(resource.AuthorityName));

            var item = resource.ToLegalAuthorityEntity();
            await _context.LegalAuthorities.AddAsync(item).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return item.Id;
        }

        public async Task UpdateAsync(int id, LegalAuthorityUpdate resource)
        {
            Guard.NotNull(resource, nameof(resource));
            Guard.NotNullOrWhiteSpace(resource.AuthorityName, nameof(resource.AuthorityName));

            var item = await _context.LegalAuthorities.FindAsync(id).ConfigureAwait(false);

            if (item == null)
            {
                throw new ArgumentException($"ID ({id}) not found.", nameof(id));
            }

            item.UpdateEntityFrom(resource);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateStatusAsync(int id, bool newActiveStatus)
        {
            var item = await _context.LegalAuthorities.FindAsync(id);
            if (item == null) throw new ArgumentException($"ID ({id}) not found.", nameof(id));
            item.Active = newActiveStatus;
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(int id) => _context.LegalAuthorities.AnyAsync(e => e.Id == id);

        public Task<bool> NameExistsAsync(string name, int? ignoreId = null) =>
            _context.LegalAuthorities.AsNoTracking()
                .AnyAsync(e => e.AuthorityName == name && e.Id != ignoreId);

        public void Dispose() => _context.Dispose();
    }
}
