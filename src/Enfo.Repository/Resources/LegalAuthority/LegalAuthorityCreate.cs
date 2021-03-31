using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Repository.Resources.LegalAuthority
{
    public class LegalAuthorityCreate
    {
        [Required(ErrorMessage = "Legal Authority Name is required.")]
        [DisplayName("Legal Authority Name")]
        public string AuthorityName { get; set; }

        public void TrimAll() => AuthorityName = AuthorityName.Trim();
    }
}