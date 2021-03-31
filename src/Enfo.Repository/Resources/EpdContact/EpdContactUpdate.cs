using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Repository.Resources.EpdContact
{
    public class EpdContactUpdate
    {
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

        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        [RegularExpression(ResourceRegex.Telephone, ErrorMessage = "Provide a valid phone number.")]
        public string Telephone { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        [RegularExpression(ResourceRegex.Email, ErrorMessage = "Provide a valid email address.")]
        public string Email { get; set; }
        
        public void TrimAll()
        {
            ContactName = ContactName?.Trim();
            Title = Title?.Trim();
            Organization = Organization?.Trim();
            Telephone = Telephone?.Trim();
            Email = Email?.Trim();
        }
    }
}