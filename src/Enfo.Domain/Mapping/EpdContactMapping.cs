using Enfo.Domain.Entities;
using Enfo.Domain.Resources.EpdContact;
using Enfo.Domain.Utils;
using JetBrains.Annotations;

namespace Enfo.Domain.Mapping
{
    public static class EpdContactMapping
    {
        public static EpdContact ToEpdContactEntity([NotNull] this EpdContactCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));

            return new EpdContact
            {
                AddressId = resource.AddressId,
                ContactName = resource.ContactName,
                Email = resource.Email,
                Organization = resource.Organization,
                Telephone = resource.Telephone,
                Title = resource.Title,
            };
        }

        public static void UpdateEntityFrom([NotNull] this EpdContact item, [NotNull] EpdContactUpdate resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.AddressId = resource.AddressId;
            item.ContactName = resource.ContactName;
            item.Email = resource.Email;
            item.Organization = resource.Organization;
            item.Telephone = resource.Telephone;
            item.Title = resource.Title;
        }

        public static EpdContactUpdate ToEpdContactUpdate(EpdContactView item) => new()
        {
            AddressId = item.Address.Id,
            IsInactiveAddress = !item.Address.Active,
            ContactName = item.ContactName,
            Email = item.Email,
            Organization = item.Organization,
            Telephone = item.Telephone,
            Title = item.Title,
        };
    }
}
