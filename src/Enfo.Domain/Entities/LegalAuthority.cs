using Enfo.Domain.Entities.Base;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Entities;

public class LegalAuthority : BaseActiveEntity
{
    public LegalAuthority() { }

    public LegalAuthority(LegalAuthorityCommand resource) =>
        this.ApplyUpdate(resource);

    [Required]
    [StringLength(100)]
    public string AuthorityName { get; set; }

    public void ApplyUpdate(LegalAuthorityCommand resource)
    {
        Guard.NotNull(resource, nameof(resource));
        AuthorityName = Guard.NotNull(resource.AuthorityName, nameof(resource.AuthorityName));
    }
}
