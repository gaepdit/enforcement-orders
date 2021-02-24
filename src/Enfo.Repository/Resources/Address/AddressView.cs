using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    }
}