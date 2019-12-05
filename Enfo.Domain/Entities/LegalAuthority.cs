using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Entities
{
    public class LegalAuthority : BaseActiveEntity
    {
        [Required]
        [StringLength(100)]
        public string AuthorityName { get; set; }
    }
}
