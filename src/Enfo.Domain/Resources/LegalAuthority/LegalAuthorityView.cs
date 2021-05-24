using System.ComponentModel;
using Enfo.Domain.Utils;
using JetBrains.Annotations;

namespace Enfo.Domain.Resources.LegalAuthority
{
    public class LegalAuthorityView
    {
        public LegalAuthorityView([NotNull] Domain.Entities.LegalAuthority item)
        {
            Guard.NotNull(item, nameof(item));

            Id = item.Id;
            Active = item.Active;
            AuthorityName = item.AuthorityName;
        }

        public int Id { get; }
        public bool Active { get; }

        [DisplayName("Legal Authority Name")]
        public string AuthorityName { get; init; }
    }
}
