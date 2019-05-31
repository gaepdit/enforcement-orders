using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Models.Models
{
    public class LegalAuthority : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Legal Authority is required")]
        [DisplayName("Legal Authority")]
        [StringLength(100)]
        public string AuthorityName { get; set; }

        [DisplayName("Order Number Template")]
        [StringLength(40)]
        public string OrderNumberTemplate { get; set; }

        public bool Active { get; set; } = true;
    }
}
