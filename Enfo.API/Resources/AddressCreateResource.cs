using Enfo.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.API.Resources
{
    public class AddressCreateResource
    {
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

        public Address NewAddress() => new Address
        {
            City = City,
            PostalCode = PostalCode,
            State = State,
            Street = Street,
            Street2 = Street2
        };
    }
}
