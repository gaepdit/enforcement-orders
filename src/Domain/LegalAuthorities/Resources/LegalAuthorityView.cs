using Enfo.Domain.LegalAuthorities.Entities;

namespace Enfo.Domain.LegalAuthorities.Resources;

public class LegalAuthorityView
{
    public LegalAuthorityView([NotNull] LegalAuthority item)
    {
        Guard.NotNull(item);

        Id = item.Id;
        Active = item.Active;
        AuthorityName = item.AuthorityName;
    }

    public int Id { get; }
    public bool Active { get; }

    [DisplayName("Legal Authority Name")]
    public string AuthorityName { get; init; }
}
