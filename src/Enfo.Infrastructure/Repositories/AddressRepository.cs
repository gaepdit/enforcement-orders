using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Enfo.Infrastructure.Contexts;
using Enfo.Repository.Mapping;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.Address;
using Enfo.Repository.Utils;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Repositories
{
    public sealed class AddressRepository : IAddressRepository
    {
        private readonly EnfoDbContext _context;
        public AddressRepository(EnfoDbContext context) => _context = Guard.NotNull(context, nameof(context));

        public async Task<AddressView> GetAsync(int id)
        {
            var item = await _context.Addresses.AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

            return item == null ? null : new AddressView(item);
        }

        public async Task<IReadOnlyList<AddressView>> ListAsync(bool includeInactive = false) =>
            await _context.Addresses.AsNoTracking()
                .Where(e => e.Active || includeInactive)
                .OrderBy(e => e.Id)
                .Select(e => new AddressView(e))
                .ToListAsync().ConfigureAwait(false);

        public async Task<int> CreateAsync(AddressCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));
            Guard.NotNullOrWhiteSpace(resource.City, nameof(resource.City));
            Guard.NotNullOrWhiteSpace(resource.State, nameof(resource.State));
            Guard.NotNullOrWhiteSpace(resource.Street, nameof(resource.Street));
            Guard.NotNullOrWhiteSpace(resource.PostalCode, nameof(resource.PostalCode));

            if (!Regex.IsMatch(resource.PostalCode, AddressCreate.PostalCodeRegex))
            {
                throw new ArgumentException($"Postal Code ({resource.PostalCode}) is not valid.", nameof(resource));
            }

            var item = resource.ToAddress();
            await _context.Addresses.AddAsync(item).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return item.Id;
        }

        public async Task UpdateAsync(int id, AddressUpdate resource)
        {
            Guard.NotNull(resource, nameof(resource));
            Guard.NotNullOrWhiteSpace(resource.City, nameof(resource.City));
            Guard.NotNullOrWhiteSpace(resource.State, nameof(resource.State));
            Guard.NotNullOrWhiteSpace(resource.Street, nameof(resource.Street));
            Guard.NotNullOrWhiteSpace(resource.PostalCode, nameof(resource.PostalCode));

            if (!Regex.IsMatch(resource.PostalCode, AddressCreate.PostalCodeRegex))
            {
                throw new ArgumentException($"Postal Code ({resource.PostalCode}) is not valid.", nameof(resource));
            }

            var item = await _context.Addresses.FindAsync(id).ConfigureAwait(false);

            if (item == null)
            {
                throw new ArgumentException($"ID ({id}) not found.", nameof(id));
            }

            item.UpdateFrom(resource);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose() => _context.Dispose();
    }
}
