using Enfo.Domain.Entities;
using Enfo.Repository.Resources.Address;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Mapping
{
    public static class AddressMapping
    {
        public static Address ToAddressEntity([NotNull] this AddressCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));

            return new Address
            {
                City = resource.City,
                PostalCode = resource.PostalCode,
                State = resource.State,
                Street = resource.Street,
                Street2 = resource.Street2,
            };
        }

        public static void UpdateEntityFrom([NotNull] this Address item, [NotNull] AddressUpdate resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.City = resource.City;
            item.PostalCode = resource.PostalCode;
            item.State = resource.State;
            item.Street = resource.Street;
            item.Street2 = resource.Street2;
        }

        public static AddressUpdate ToAddressUpdate(AddressView item) => new()
        {
            City = item.City,
            PostalCode = item.PostalCode,
            State = item.State,
            Street = item.Street,
            Street2 = item.Street2,
        };
    }
}