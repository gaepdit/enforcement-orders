using Enfo.Domain.EpdContacts;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.EfRepository.Contexts;
using GaEpd.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace Enfo.EfRepository.Repositories;

public sealed class EpdContactRepository(EnfoDbContext context) : IEpdContactRepository
{
    private readonly EnfoDbContext _context = Guard.NotNull(context);

    public async Task<EpdContactViewDto> GetAsync(int id, CancellationToken token)
    {
        var item = await _context.EpdContacts.AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken: token).ConfigureAwait(false);

        return item == null ? null : new EpdContactViewDto(item);
    }

    public async Task<IReadOnlyList<EpdContactViewDto>> ListAsync(bool includeInactive = false,
        CancellationToken token = default) =>
        await _context.EpdContacts.AsNoTracking()
            .Where(e => e.Active || includeInactive)
            .OrderBy(e => e.ContactName)
            .Select(e => new EpdContactViewDto(e))
            .ToListAsync(cancellationToken: token).ConfigureAwait(false);

    public async Task<int> CreateAsync(EpdContactCommand resource, CancellationToken token)
    {
        var item = new EpdContact(resource);
        _context.EpdContacts.Add(item);
        await _context.SaveChangesAsync(token).ConfigureAwait(false);
        return item.Id;
    }

    public async Task UpdateAsync(int id, EpdContactCommand resource, CancellationToken token)
    {
        var item = (await _context.EpdContacts.FindAsync([id], token).ConfigureAwait(false))
                   ?? throw new ArgumentException($"ID ({id}) not found.", nameof(resource));
        resource.TrimAll();
        item.ApplyUpdate(resource);
        await _context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task UpdateStatusAsync(int id, bool newActiveStatus, CancellationToken token)
    {
        var item = await _context.EpdContacts.FindAsync([id], token).ConfigureAwait(false)
                   ?? throw new ArgumentException($"ID ({id}) not found.", nameof(id));
        item.Active = newActiveStatus;
        await _context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken token) =>
        _context.EpdContacts.AnyAsync(e => e.Id == id, cancellationToken: token);

    public void Dispose() => _context.Dispose();
}
