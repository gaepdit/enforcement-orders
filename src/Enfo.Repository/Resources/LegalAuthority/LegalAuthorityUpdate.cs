using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.LegalAuthority
{
    public class LegalAuthorityUpdate
    {
        public LegalAuthorityUpdate() { }

        public LegalAuthorityUpdate([NotNull] LegalAuthorityView item)
        {
            Guard.NotNull(item, nameof(item));
            AuthorityName = item.AuthorityName;
        }

        [Required(ErrorMessage = "Legal Authority Name is required.")]
        [DisplayName("Legal Authority Name")]
        public string AuthorityName { get; set; }

        public void TrimAll() => AuthorityName = AuthorityName.Trim();
    }
}