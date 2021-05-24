using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Resources.LegalAuthority
{
    public class LegalAuthorityCommand
    {
        public LegalAuthorityCommand() { }

        public LegalAuthorityCommand(LegalAuthorityView item) =>
            AuthorityName = item.AuthorityName;

        [Required(ErrorMessage = "Legal Authority Name is required.")]
        [DisplayName("Legal Authority Name")]
        public string AuthorityName { get; set; }

        public void TrimAll() => AuthorityName = AuthorityName?.Trim();
    }
}
