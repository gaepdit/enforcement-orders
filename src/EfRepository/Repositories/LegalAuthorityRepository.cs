using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.EfRepository.Contexts;
using GaEpd.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace Enfo.EfRepository.Repositories;

public sealed class LegalAuthorityRepository(EnfoDbContext context) : ILegalAuthorityRepository
{
    private readonly EnfoDbContext _context = Guard.NotNull(context);

    public async Task<LegalAuthorityViewDto> GetAsync(int id, CancellationToken token)
    {
        var item = await _context.LegalAuthorities.AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken: token).ConfigureAwait(false);

        return item == null ? null : new LegalAuthorityViewDto(item);
    }

    public async Task<IReadOnlyList<LegalAuthorityViewDto>> ListAsync(bool includeInactive = false,
        CancellationToken token = default) =>
        await _context.LegalAuthorities.AsNoTracking()
            .Where(e => e.Active || includeInactive)
            .OrderBy(e => e.AuthorityName).ThenBy(e => e.Id)
            .Select(e => new LegalAuthorityViewDto(e))
            .ToListAsync(token).ConfigureAwait(false);


    public async Task<int> CreateAsync(LegalAuthorityCreateDto resource, CancellationToken token)
    {
        var item = new LegalAuthority { AuthorityName = resource.AuthorityName.Trim() };
        _context.LegalAuthorities.Add(item);
        await _context.SaveChangesAsync(token).ConfigureAwait(false);

        return item.Id;
    }

    public async Task UpdateAsync(int id, LegalAuthorityUpdateDto resource, CancellationToken token)
    {
        var item = (await _context.LegalAuthorities.FindAsync([id], token).ConfigureAwait(false))
                   ?? throw new ArgumentException($"ID ({id}) not found.", nameof(resource));

        item.AuthorityName = resource.AuthorityName.Trim();
        item.Active = resource.Active;
        await _context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken token) =>
        _context.LegalAuthorities.AnyAsync(e => e.Id == id, cancellationToken: token);

    public Task<bool> NameExistsAsync(string name, CancellationToken token) =>
        _context.LegalAuthorities.AsNoTracking().AnyAsync(e => e.AuthorityName == name, token);

    public async Task<int?> FindIdByNameAsync(string name, CancellationToken token) =>
        (await _context.LegalAuthorities.AsNoTracking()
            .SingleOrDefaultAsync(e => e.AuthorityName == name, token)
            .ConfigureAwait(false))?.Id;

    public void Dispose() => _context.Dispose();
}
