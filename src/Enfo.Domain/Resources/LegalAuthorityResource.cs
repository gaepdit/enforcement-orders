using Enfo.Domain.Models;
using System.ComponentModel;

namespace Enfo.Domain.Resources
{
    public class LegalAuthorityResource
    {
        public int Id { get; set; }

        [DisplayName("Legal Authority")]
        public string AuthorityName { get; set; }

        [DisplayName("Order Number Template")]
        public string OrderNumberTemplate { get; set; }

        public bool Active { get; set; } = true;

        public LegalAuthorityResource() { }

        public LegalAuthorityResource(LegalAuthority legalAuthority)
        {
            if (legalAuthority != null)
            {
                Id = legalAuthority.Id;                
                Active = legalAuthority.Active;
                AuthorityName = legalAuthority.AuthorityName;
                OrderNumberTemplate = legalAuthority.OrderNumberTemplate;
            }
        }
    }
}
