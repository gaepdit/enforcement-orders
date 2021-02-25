using System.ComponentModel;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Resources.Address
{
    public class AddressView
    {
        public AddressView(Domain.Entities.Address item)
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

        public int Id { get; set; }
        public bool Active { get; set; }

        [DisplayName("Street Address")]
        public string Street { get; set; }

        [DisplayName("Apt / Suite / Other")]
        public string Street2 { get; set; }

        public string City { get; set; }
        public string State { get; set; } = "GA";

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
    }
}