using Enfo.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.API.Resources
{
    public class LegalAuthorityResource
    {
        public int Id { get; set; }
        public bool Active { get; set; } = true;

        [DisplayName("Legal Authority")]
        [Required(ErrorMessage = "Legal Authority name is required")]
        public string AuthorityName { get; set; }

        public LegalAuthorityResource() { }
        public LegalAuthorityResource(LegalAuthority item)
        {
            if (item != null)
            {
                Id = item.Id;
                Active = item.Active;
                AuthorityName = item.AuthorityName;
            }
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
                item.UpdatedDate = DateTime.Now;
            }
        }
    }
}
