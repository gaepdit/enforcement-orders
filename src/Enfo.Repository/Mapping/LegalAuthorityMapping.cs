using Enfo.Domain.Entities;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Mapping
{
    public static class LegalAuthorityMapping
    {
        public static LegalAuthority ToLegalAuthority(this LegalAuthorityCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));

            return new LegalAuthority
            {
                AuthorityName = resource.AuthorityName
            };
        }

        public static void UpdateFrom(this LegalAuthority item, LegalAuthorityUpdate resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.Active = resource.Active;
            item.AuthorityName = resource.AuthorityName;
        }
    }
}