using System.ComponentModel;
using Enfo.Domain.Utils;
using JetBrains.Annotations;

namespace Enfo.Domain.Resources.EpdContact
{
    public class EpdContactView
    {
        public EpdContactView([NotNull] Entities.EpdContact item)
        {
            Guard.NotNull(item, nameof(item));

            Id = item.Id;
            Active = item.Active;
            ContactName = item.ContactName;
            Title = item.Title;
            Organization = item.Organization;
            Telephone = item.Telephone;
            Email = item.Email;
            City = item.City;
            PostalCode = item.PostalCode;
            State = item.State;
            Street = item.Street;
            Street2 = item.Street2;
        }

        public int Id { get; }
        public bool Active { get; }

        [DisplayName("Contact Name")]
        public string ContactName { get; }

        public string Title { get; }
        public string Organization { get; }
        public string Telephone { get; }
        public string Email { get; }

        [DisplayName("Street Address")]
        public string Street { get; }

        [DisplayName("Apt / Suite / Other")]
        public string Street2 { get; }

        public string City { get; }
        public string State { get; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; }

        public string AsLinearString
        {
            get
            {
                var cityState = new[] {City, State}.ConcatNonEmptyStrings(", ");
                var cityStateZip = new[] {cityState, PostalCode}.ConcatNonEmptyStrings(" ");
                return new[] {ContactName, Street, Street2, cityStateZip}.ConcatNonEmptyStrings(", ");
            }
        }
    }
}
