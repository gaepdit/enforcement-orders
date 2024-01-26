using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Infrastructure.Contexts;
using GaEpd.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Repositories;

public sealed class LegalAuthorityRepository : ILegalAuthorityRepository
{
    private readonly EnfoDbContext _context;
    public LegalAuthorityRepository(EnfoDbContext context) => _context = Guard.NotNull(context);

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


    public async Task<int> CreateAsync(LegalAuthorityCommand resource)
    {
        var item = new LegalAuthority(resource);
        await _context.LegalAuthorities.AddAsync(item).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return item.Id;
    }

    public async Task UpdateAsync(LegalAuthorityCommand resource)
    {
        var item = (await _context.LegalAuthorities.FindAsync(resource.Id).ConfigureAwait(false))
            ?? throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

        if (!item.Active) throw new ArgumentException("Only active items can be edited.", nameof(resource));

        resource.TrimAll();
        item.ApplyUpdate(resource);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateStatusAsync(int id, bool newActiveStatus)
    {
        var item = await _context.LegalAuthorities.FindAsync(id).ConfigureAwait(false)
            ?? throw new ArgumentException($"ID ({id}) not found.", nameof(id));
        item.Active = newActiveStatus;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public Task<bool> ExistsAsync(int id) => _context.LegalAuthorities.AnyAsync(e => e.Id == id);

    public Task<bool> NameExistsAsync(string name) =>
        _context.LegalAuthorities.AsNoTracking()
            .AnyAsync(e => e.AuthorityName == name);

    public void Dispose() => _context.Dispose();
}
