﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Resources.Address
{
    public class AddressCreate
    {
        [DisplayName("Street Address")]
        [Required(ErrorMessage = "Street Address is required.")]
        [StringLength(100)]
        public string Street { get; set; }

        [DisplayName("Apt / Suite / Other")]
        [StringLength(100)]
        public string Street2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(2)]
        [DefaultValue("GA")]
        public string State { get; set; } = "GA";

        [DisplayName("Postal Code")]
        [Required(ErrorMessage = "Postal Code is required.")]
        [DataType(DataType.PostalCode)]
        [StringLength(10)]
        [RegularExpression(ResourceRegex.PostalCode, ErrorMessage = "Provide a valid US ZIP Code.")]
        public string PostalCode { get; set; }

        public void TrimAll()
        {
            Street = Street?.Trim();
            Street2 = Street2?.Trim();
            City = City?.Trim();
            State = State?.Trim();
            PostalCode = PostalCode?.Trim();
        }
    }
}