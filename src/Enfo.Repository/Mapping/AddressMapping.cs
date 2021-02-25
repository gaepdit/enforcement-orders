﻿using Enfo.Domain.Entities;
using Enfo.Repository.Resources.Address;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Mapping
{
    public static class AddressMapping
    {
        public static Address ToAddress(this AddressCreate resource)
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

        public static void UpdateFrom(this Address item, AddressUpdate resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.Active = resource.Active;
            item.City = resource.City;
            item.PostalCode = resource.PostalCode;
            item.State = resource.State;
            item.Street = resource.Street;
            item.Street2 = resource.Street2;
        }
    }
}