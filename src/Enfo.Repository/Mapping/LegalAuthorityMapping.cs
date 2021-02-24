using Enfo.Domain.Entities;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Mapping
{
    public static class LegalAuthorityMapping
    {
        public static LegalAuthorityView ToLegalAuthorityView(this LegalAuthority item)
        {
            Guard.NotNull(item, nameof(item));

            return new LegalAuthorityView()
            {
                Id = item.Id,
                Active = item.Active,
                AuthorityName = item.AuthorityName,
            };
        }

        public static LegalAuthority ToLegalAuthority(this LegalAuthorityCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));

            return new LegalAuthority()
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