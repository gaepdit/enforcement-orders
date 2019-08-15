using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Entities
{
    public class LegalAuthority : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string AuthorityName { get; set; }
    }
}
