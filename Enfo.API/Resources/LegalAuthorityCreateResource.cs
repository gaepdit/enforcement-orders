using Enfo.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.API.Resources
{
    public class LegalAuthorityCreateResource
    {
        [DisplayName("Legal Authority")]
        [Required(ErrorMessage = "Legal Authority name is required")]
        public string AuthorityName { get; set; }

        public LegalAuthority NewLegalAuthority()
        {
            return new LegalAuthority()
            {
                AuthorityName = AuthorityName
            };
        }
    }
}
