using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Repository.Resources.Address
{
    public class AddressUpdate
    {
        public bool Active { get; set; }

        [DisplayName("Street Address")]
        [Required(ErrorMessage = "Street is required")]
        [StringLength(100)]
        public string Street { get; set; }

        [DisplayName("Apt / Suite / Other")]
        [StringLength(100)]
        public string Street2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(2)]
        [DefaultValue("GA")]
        public string State { get; set; } = "GA";

        [DisplayName("Postal Code")]
        [StringLength(10)]
        [RegularExpression("^\\d{5}(-\\d{4})?$", ErrorMessage = "Valid US Postal Code is required")]
        public string PostalCode { get; set; }
    }
}