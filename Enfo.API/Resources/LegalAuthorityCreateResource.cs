using Enfo.Domain.Entities;
using System.ComponentModel;

namespace Enfo.API.Resources
{
    public class LegalAuthorityCreateResource
    {
        [DisplayName("Legal Authority")]
        public string AuthorityName { get; set; }

        [DisplayName("Order Number Template")]
        public string OrderNumberTemplate { get; set; }

        public LegalAuthority NewLegalAuthority()
        {
            return new LegalAuthority()
            {
                AuthorityName = AuthorityName,
                OrderNumberTemplate = OrderNumberTemplate
            };

        }
    }
}
