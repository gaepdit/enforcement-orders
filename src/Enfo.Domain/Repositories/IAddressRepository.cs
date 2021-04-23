using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Resources.Address;

namespace Enfo.Domain.Repositories
{
    public interface IAddressRepository : IDisposable
    {
        Task<AddressView> GetAsync(int id);
        Task<IReadOnlyList<AddressView>> ListAsync(bool includeInactive = false);
        Task<int> CreateAsync(AddressCreate resource);
        Task UpdateAsync(int id, AddressUpdate resource);
        Task UpdateStatusAsync(int id, bool newActiveStatus);
        Task<bool> ExistsAsync(int id);
    }
}
