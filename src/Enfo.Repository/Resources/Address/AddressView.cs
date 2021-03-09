using System.ComponentModel;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.Address
{
    public class AddressView
    {
        public AddressView([NotNull] Domain.Entities.Address item)
        {
            Guard.NotNull(item, nameof(item));

            Active = item.Active;
            City = item.City;
            Id = item.Id;
            PostalCode = item.PostalCode;
            State = item.State;
            Street = item.Street;
            Street2 = item.Street2;
        }

        public int Id { get; }
        public bool Active { get; }

        [DisplayName("Street Address")]
        public string Street { get; }

        [DisplayName("Apt / Suite / Other")]
        public string Street2 { get; }

        public string City { get; }
        public string State { get; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; }
    }
}