using Enfo.Domain.Users.Entities;

namespace EnfoTests.TestData;

internal static class UsersData
{
    public static List<ApplicationUser> Users { get; } =
    [
        new()
        {
            Id = Guid.NewGuid(),
            GivenName = "Local",
            FamilyName = "User",
            Email = "local.user@example.net",
            ObjectId = Guid.NewGuid().ToString(),
        },
    ];
}
