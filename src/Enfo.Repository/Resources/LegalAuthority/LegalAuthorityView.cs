using System.ComponentModel;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.LegalAuthority
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

        [DisplayName("Legal Authority Name")]
        public string AuthorityName { get; set; }

        public bool Active { get; }
    }
}