using Enfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        public AddressResource() { }

        public AddressResource(Address item)
        {
            if (item != null)
            {
                Id = item.Id;
                Active = item.Active;
                Street = item.Street;
                Street2 = item.Street2;
                City = item.City;
                State = item.State;
                PostalCode = item.PostalCode;
            }
        }

        public Address NewAddress()
        {
            return new Address
            {
                Active = Active,
                City = City,
                PostalCode = PostalCode,
                State = State,
                Street = Street,
                Street2 = Street2
            };
        }
    }

    public static class AddressExtension
    {
        public static void UpdateFrom(this Address item, AddressResource resource)
        {
            if (resource != null)
            {
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
}
