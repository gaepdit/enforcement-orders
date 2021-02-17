using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Resources
{
    public class LegalAuthorityUpdateResource
    {
        public bool Active { get; set; } = true;

        [DisplayName("Legal Authority")]
        [Required(ErrorMessage = "Legal Authority name is required")]
        public string AuthorityName { get; set; }
    }

    public static class LegalAuthorityExtension
    {
        public static void UpdateFrom(this LegalAuthority item, LegalAuthorityUpdateResource resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.Active = resource.Active;
            item.AuthorityName = resource.AuthorityName;
            item.UpdatedDate = DateTime.Now;
        }
    }
}
