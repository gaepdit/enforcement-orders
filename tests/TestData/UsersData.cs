using Enfo.Domain.Users.Entities;

namespace EnfoTests.TestData;

internal static class UsersData
{
    public static readonly List<ApplicationUser> Users = new()
    {
        new ApplicationUser
        {
            Id = Guid.NewGuid(),
            GivenName = "Local",
            FamilyName = "User",
            Email = "local.user@example.net",
        },
    };
}
