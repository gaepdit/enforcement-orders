using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Querying;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.Address;

namespace Enfo.Repository.Interfaces
{
    public interface IAddressRepository : IDisposable
    {
        Task<AddressView> GetAsync(int id);
        Task<IReadOnlyList<AddressView>> ListAsync(bool includeInactive);
        Task<int> CreateAsync(AddressCreate resource);
        Task UpdateAsync(int id, AddressUpdate resource);
    }
}