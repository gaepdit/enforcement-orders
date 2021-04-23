using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities.Base;

namespace Enfo.Domain.Entities
{
    public class Address : BaseActiveEntity
    {
        // Postal (mailable) addresses only

        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(100)]
        public string Street2 { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [DefaultValue("GA")]
        [StringLength(2)]
        public string State { get; set; } = "GA";

        [StringLength(10)]
        public string PostalCode { get; set; }
    }
}
