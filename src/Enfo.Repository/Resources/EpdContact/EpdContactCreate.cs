using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Repository.Resources.EpdContact
{
    public class EpdContactCreate
    {
        [DisplayName("Contact Name")]
        [Required(ErrorMessage = "Contact Name is required.")]
        [StringLength(50)]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Organization is required.")]
        [StringLength(100)]
        public string Organization { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is required.")]
        public int AddressId { get; set; }

        [StringLength(50)]
        [RegularExpression(ResourceRegex.Telephone, ErrorMessage = "Provide a valid phone number.")]
        public string Telephone { get; set; }

        [StringLength(100)]
        [RegularExpression(ResourceRegex.Email, ErrorMessage = "Provide a valid email address.")]
        public string Email { get; set; }
    }
}