using Enfo.Domain.Entities;
using Enfo.Repository.Resources.Address;
using Enfo.Repository.Resources.EpdContact;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Mapping
{
    public static class EpdContactMapping
    {
        public static EpdContactView ToEpdContactView(this EpdContact item)
        {
            Guard.NotNull(item, nameof(item));

            return new EpdContactView()
            {
                Id = item.Id,
                Active = item.Active,
                ContactName = item.ContactName,
                Title = item.Title,
                Organization = item.Organization,
                Address = new AddressView(item.Address),
                AddressId = item.AddressId,
                Telephone = item.Telephone,
                Email = item.Email,
            };
        }

        public static EpdContact ToEpdContact(this EpdContactCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));

            return new EpdContact()
            {
                AddressId = resource.AddressId,
                ContactName = resource.ContactName,
                Email = resource.Email,
                Organization = resource.Organization,
                Telephone = resource.Telephone,
                Title = resource.Title,
            };
        }

        public static void UpdateFrom(this EpdContact item, EpdContactUpdate resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.Active = resource.Active;
            item.AddressId = resource.AddressId;
            item.ContactName = resource.ContactName;
            item.Email = resource.Email;
            item.Organization = resource.Organization;
            item.Telephone = resource.Telephone;
            item.Title = resource.Title;
        }
    }
}