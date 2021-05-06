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
                ContactName = resource.ContactName,
                Email = resource.Email,
                Organization = resource.Organization,
                Telephone = resource.Telephone,
                Title = resource.Title,
                City = resource.City,
                PostalCode = resource.PostalCode,
                State = resource.State,
                Street = resource.Street,
                Street2 = resource.Street2,
            };
        }

        public static void UpdateEntityFrom([NotNull] this EpdContact item, [NotNull] EpdContactUpdate resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.ContactName = resource.ContactName;
            item.Email = resource.Email;
            item.Organization = resource.Organization;
            item.Telephone = resource.Telephone;
            item.Title = resource.Title;
            item.City = resource.City;
            item.PostalCode = resource.PostalCode;
            item.State = resource.State;
            item.Street = resource.Street;
            item.Street2 = resource.Street2;
        }

        public static EpdContactUpdate ToEpdContactUpdate(EpdContactView item) => new()
        {
            ContactName = item.ContactName,
            Email = item.Email,
            Organization = item.Organization,
            Telephone = item.Telephone,
            Title = item.Title,
            City = item.City,
            PostalCode = item.PostalCode,
            State = item.State,
            Street = item.Street,
            Street2 = item.Street2,
        };
    }
}
