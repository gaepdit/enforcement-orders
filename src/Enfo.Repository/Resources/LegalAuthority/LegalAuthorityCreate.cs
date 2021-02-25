using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Repository.Resources.LegalAuthority
{
    public class LegalAuthorityCreate
    {
        [Required(ErrorMessage = "Legal Authority name is required.")]
        [DisplayName("Legal Authority")]
        public string AuthorityName { get; set; }
    }
}