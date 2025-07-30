using Enfo.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace EnfoTests.TestData;

internal static class UserData
{
    private const string UserEmail = "local.user@example.net";

    public static List<ApplicationUser> GetUsers { get; } =
    [
        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            GivenName = "Local",
            FamilyName = "User",
            Email = UserEmail,
            UserName = UserEmail,
            NormalizedEmail = UserEmail.ToUpperInvariant(),
            NormalizedUserName = UserEmail.ToUpperInvariant(),
        },
    ];

    public static IEnumerable<IdentityRole<Guid>> GetRoles { get; } =
        AppRole.AllRoles.Select(pair => new IdentityRole<Guid>(pair.Value.Name)
            { Id = Guid.NewGuid(), NormalizedName = pair.Key.ToUpperInvariant() }).ToList();
}
