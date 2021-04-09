using Enfo.Domain.Entities;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Mapping
{
    public static class LegalAuthorityMapping
    {
        public static LegalAuthority ToLegalAuthorityEntity([NotNull] this LegalAuthorityCreate resource)
        {
            Guard.NotNull(resource, nameof(resource));

            return new LegalAuthority
            {
                AuthorityName = resource.AuthorityName
            };
        }

        public static void UpdateEntityFrom([NotNull] this LegalAuthority item, [NotNull] LegalAuthorityUpdate resource)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(resource, nameof(resource));

            item.AuthorityName = resource.AuthorityName;
        }

        public static LegalAuthorityUpdate ToLegalAuthorityUpdate(LegalAuthorityView item) => new()
            {AuthorityName = item.AuthorityName};
    }
}