﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EpdContact;
using Enfo.Domain.Utils;
using Enfo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Repositories
{
    public sealed class EpdContactRepository : IEpdContactRepository
    {
        private readonly EnfoDbContext _context;
        public EpdContactRepository(EnfoDbContext context) => _context = Guard.NotNull(context, nameof(context));

        public async Task<EpdContactView> GetAsync(int id)
        {
            var item = await _context.EpdContacts.AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

            return item == null ? null : new EpdContactView(item);
        }

        public async Task<IReadOnlyList<EpdContactView>> ListAsync(bool includeInactive = false) =>
            await _context.EpdContacts.AsNoTracking()
                .Where(e => e.Active || includeInactive)
                .OrderBy(e => e.ContactName)
                .Select(e => new EpdContactView(e))
                .ToListAsync().ConfigureAwait(false);

        public async Task<int> CreateAsync(EpdContactCommand resource)
        {
            var item = new EpdContact(resource);
            await _context.EpdContacts.AddAsync(item).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return item.Id;
        }

        public async Task UpdateAsync(EpdContactCommand resource)
        {
            var item = (await _context.EpdContacts.FindAsync(resource.Id).ConfigureAwait(false))
                ?? throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

            resource.TrimAll();
            item.ApplyUpdate(resource);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateStatusAsync(int id, bool newActiveStatus)
        {
            var item = await _context.EpdContacts.FindAsync(id);
            if (item == null) throw new ArgumentException($"ID ({id}) not found.", nameof(id));
            item.Active = newActiveStatus;
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(int id) => _context.EpdContacts.AnyAsync(e => e.Id == id);

        public void Dispose() => _context.Dispose();
    }
}
