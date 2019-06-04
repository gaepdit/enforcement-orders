using Enfo.Domain.Entities;
using System.ComponentModel;

namespace Enfo.API.Resources
{
    public class LegalAuthorityResource
    {
        public int Id { get; set; }

        [DisplayName("Legal Authority")]
        public string AuthorityName { get; set; }

        [DisplayName("Order Number Template")]
        public string OrderNumberTemplate { get; set; }

        public bool Active { get; set; }

        public LegalAuthorityResource() { }

        public LegalAuthorityResource(LegalAuthority entity)
        {
            if (entity != null)
            {
                Id = entity.Id;
                Active = entity.Active;
                AuthorityName = entity.AuthorityName;
                OrderNumberTemplate = entity.OrderNumberTemplate;
            }
        }
    }
}
