using Enfo.Domain.LegalAuthorities.Entities;

// ReSharper disable StringLiteralTypo

namespace Enfo.LocalRepository.LegalAuthorities;

internal static class LegalAuthorityData
{
    public static readonly List<LegalAuthority> LegalAuthorities = new()
    {
        new LegalAuthority { Id = 1, Active = true, AuthorityName = "Air Quality Act" },
        new LegalAuthority { Id = 2, Active = true, AuthorityName = "Asbestos Safety Act" },
        new LegalAuthority { Id = 3, Active = false, AuthorityName = "Obsolete Act" },
    };
}
