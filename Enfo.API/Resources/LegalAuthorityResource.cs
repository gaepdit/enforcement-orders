using Enfo.Domain.Entities;
using System;
using System.ComponentModel;

namespace Enfo.API.Resources
{
    public class LegalAuthorityResource
    {
        public int Id { get; set; }
        public bool Active { get; set; } = true;

        [DisplayName("Legal Authority")]
        public string AuthorityName { get; set; }

        [DisplayName("Order Number Template")]
        public string OrderNumberTemplate { get; set; }

        public LegalAuthorityResource() { }

        public LegalAuthorityResource(LegalAuthority item)
        {
            if (item != null)
            {
                Id = item.Id;
                Active = item.Active;
                AuthorityName = item.AuthorityName;
                OrderNumberTemplate = item.OrderNumberTemplate;
            }
        }

        public LegalAuthority NewLegalAuthority()
        {
            return new LegalAuthority()
            {
                AuthorityName = AuthorityName,
                OrderNumberTemplate = OrderNumberTemplate,
                Active = Active
            };

        }
    }

    public static class LegalAuthorityExtension
    {
        public static void UpdateFrom(this LegalAuthority item, LegalAuthorityResource resource)
        {
            if (resource != null)
            {
                item.Active = resource.Active;
                item.AuthorityName = resource.AuthorityName;
                item.OrderNumberTemplate = resource.OrderNumberTemplate;
                item.UpdatedDate = DateTime.Now;
            }
        }
    }
}
