using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Entities
{
    public class EpdContact : BaseActiveEntity
    {
        [Required]
        [StringLength(50)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Organization { get; set; }

        [Required]
        public Address Address { get; set; }
        public int? AddressId { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
    }
}
