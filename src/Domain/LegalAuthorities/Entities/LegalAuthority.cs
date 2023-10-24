using Enfo.Domain.BaseEntities;
using Enfo.Domain.LegalAuthorities.Resources;

namespace Enfo.Domain.LegalAuthorities.Entities;

public class LegalAuthority : IdentifiedEntity, IAuditable
{
    public LegalAuthority() { }

    public LegalAuthority(LegalAuthorityCommand resource) =>
        this.ApplyUpdate(resource);

    [Required]
    [StringLength(100)]
    public string AuthorityName { get; set; }

    public bool Active { get; set; } = true;

    public void ApplyUpdate(LegalAuthorityCommand resource)
    {
        Guard.NotNull(resource);
        AuthorityName = Guard.NotNull(resource.AuthorityName);
    }
}
