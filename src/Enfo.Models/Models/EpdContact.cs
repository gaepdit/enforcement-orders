using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Models
{
    public class EpdContact : ModelBase
    {
        [DisplayName("Contact Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Contact Name is required")]
        public string ContactName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Organization is required")]
        public string Organization { get; set; }

        public virtual Address Address { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public int? AddressId { get; set; }

        [RegularExpression("^\\D?(\\d{3})\\D?\\D?(\\d{3})\\D?(\\d{4})$", ErrorMessage = "Please enter valid a phone number")]
        [StringLength(50)]
        public string Telephone { get; set; }

        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please provide a valid email address")]
        [StringLength(100)]
        public string Email { get; set; }
        
        public bool Active { get; set; } = true;
    }
}
