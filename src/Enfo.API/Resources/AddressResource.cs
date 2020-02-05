using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.API.Resources
{
    public class AddressResource
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        [DisplayName("Street Address")]
        [StringLength(100)]
        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [DisplayName("Apt / Suite / Other")]
        [StringLength(100)]
        public string Street2 { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [DefaultValue("GA")]
        [StringLength(2)]
        public string State { get; set; } = "GA";

        [DisplayName("Postal Code")]
        [StringLength(10)]
        [RegularExpression("^\\d{5}(-\\d{4})?$",
            ErrorMessage = "Valid US Postal Code is required")]
        public string PostalCode { get; set; }

        public AddressResource(Address item)
        {
            Check.NotNull(item, nameof(item));

            Id = item.Id;
            Active = item.Active;
            Street = item.Street;
            Street2 = item.Street2;
            City = item.City;
            State = item.State;
            PostalCode = item.PostalCode;
        }
    }
}
