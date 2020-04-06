using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.API.Resources
{
    public class AddressUpdateResource
    {
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

    public static class AddressExtension
    {
        public static void UpdateFrom(this Address item, AddressUpdateResource resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.Active = resource.Active;
            item.City = resource.City;
            item.PostalCode = resource.PostalCode;
            item.State = resource.State;
            item.Street = resource.Street;
            item.Street2 = resource.Street2;
            item.UpdatedDate = DateTime.Now;
        }
    }
}
