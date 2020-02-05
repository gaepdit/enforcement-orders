using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;

namespace Enfo.API.Resources
{
    public class LegalAuthorityCreateResource
    {
        [DisplayName("Legal Authority")]
        [Required(ErrorMessage = "Legal Authority name is required")]
        public string AuthorityName { get; set; }

        public LegalAuthority NewLegalAuthority() => new LegalAuthority()
        {
            AuthorityName = AuthorityName
        };
    }
}
