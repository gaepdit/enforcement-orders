using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

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
            Check.NotNull(item, nameof(item));

            Id = item.Id;
            Active = item.Active;
            AuthorityName = item.AuthorityName;
        }
    }
}
