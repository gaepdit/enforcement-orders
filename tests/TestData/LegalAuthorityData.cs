using Enfo.Domain.LegalAuthorities.Entities;

// ReSharper disable StringLiteralTypo

namespace TestData;

internal static class LegalAuthorityData
{
    public static LegalAuthority GetLegalAuthority(int id) =>
        LegalAuthorities.SingleOrDefault(e => e.Id == id);

    public static readonly List<LegalAuthority> LegalAuthorities = new()
    {
        new LegalAuthority { Id = 1, Active = true, AuthorityName = "Air Quality Act" },
        new LegalAuthority { Id = 2, Active = true, AuthorityName = "Asbestos Safety Act" },
        new LegalAuthority { Id = 3, Active = false, AuthorityName = "Obsolete Act" },
    };
}
