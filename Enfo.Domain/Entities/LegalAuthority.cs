using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Entities
{
    public class LegalAuthority : BaseEntity
    {
        [Required(ErrorMessage = "Legal Authority is required")]
        [DisplayName("Legal Authority")]
        [StringLength(100)]
        public string AuthorityName { get; set; }

        [DisplayName("Order Number Template")]
        [StringLength(40)]
        public string OrderNumberTemplate { get; set; }
    }
}
