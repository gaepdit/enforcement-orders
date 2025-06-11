using Enfo.Domain.LegalAuthorities.Entities;

// ReSharper disable StringLiteralTypo

namespace EnfoTests.TestData;

internal static class LegalAuthorityData
{
    public static LegalAuthority GetLegalAuthority(int id) =>
        LegalAuthorities.SingleOrDefault(e => e.Id == id);

    public static List<LegalAuthority> LegalAuthorities { get; } =
    [
        new() { Id = 1, Active = true, AuthorityName = "Air Quality Act" },
        new() { Id = 2, Active = true, AuthorityName = "Asbestos Safety Act" },
        new() { Id = 3, Active = false, AuthorityName = "Obsolete Act" },
    ];
}
