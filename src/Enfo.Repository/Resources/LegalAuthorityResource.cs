using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Resources
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
            Guard.NotNull(item, nameof(item));

            Id = item.Id;
            Active = item.Active;
            AuthorityName = item.AuthorityName;
        }
    }
}
