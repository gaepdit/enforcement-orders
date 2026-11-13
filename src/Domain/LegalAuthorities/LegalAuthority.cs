using Enfo.Domain.BaseEntities;

namespace Enfo.Domain.LegalAuthorities;

public class LegalAuthority : IdentifiedEntity, IAuditable
{
    [Required]
    [StringLength(100)]
    public string AuthorityName { get; set; }

    public bool Active { get; set; } = true;
}
