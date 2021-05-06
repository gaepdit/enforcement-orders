using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities.Base;

namespace Enfo.Domain.Entities
{
    public class EpdContact : BaseActiveEntity
    {
        [Required]
        [StringLength(250)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Organization { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
        
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
