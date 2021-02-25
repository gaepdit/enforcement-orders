using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Repository.Resources.EpdContact
{
    public class EpdContactUpdate
    {
        public bool Active { get; set; }

        [DisplayName("Contact Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Contact Name is required.")]
        public string ContactName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Organization is required.")]
        public string Organization { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is required.")]
        public int? AddressId { get; set; }

        [RegularExpression(ResourceRegex.Telephone, ErrorMessage = "Provide a valid phone number.")]
        [StringLength(50)]
        public string Telephone { get; set; }

        [RegularExpression(ResourceRegex.Email, ErrorMessage = "Provide a valid email address.")]
        [StringLength(100)]
        public string Email { get; set; }
    }
}