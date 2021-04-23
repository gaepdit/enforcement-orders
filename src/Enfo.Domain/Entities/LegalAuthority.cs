using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities.Base;

namespace Enfo.Domain.Entities
{
    public class LegalAuthority : BaseActiveEntity
    {
        [Required]
        [StringLength(100)]
        public string AuthorityName { get; set; }
    }
}
